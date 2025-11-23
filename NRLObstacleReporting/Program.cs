using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Repositories;
using NRLObstacleReporting.Repositories.IdentityStore;
using NRLObstacleReporting.StartupTests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var internalConnectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION");
var internalMariaDbConnection = new MySqlConnection(internalConnectionString);

builder.Services.AddSingleton<IObstacleRepository, ObstacleRepository>();
builder.Services.AddSingleton<IDraftRepository, DraftRepository>();
builder.Services.AddSingleton<IRegistrarRepository, RegistrarRepository>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(internalConnectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(internalConnectionString)));
});

builder.Services.AddAutoMapper(typeof(Program));


IStartupDatabaseTest[] databaseTests =
[
    InternalDatabaseConnectionTest.GetInstance(),
    InternalDatabaseReadWriteTest.GetInstance()
];

foreach (var testclass in databaseTests)
{
    testclass.InvokeAllTests();
}

// Persist DataProtection keys to disk so antiforgery/cookie tokens survive restarts.
// In Docker, mount a volume to this path or change to a shared path.
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys");
Directory.CreateDirectory(keysFolder);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("NRLObstacleReporting");

SetupAuthentication(builder);

// hide server header
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Security headers middleware (placed early so static files get the header too)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";

    // Content-Security-Policy:
    // - 'self' covers scripts served from your app (e.g. /js/MapScripts/*.js)
    // - add external CDNs and image/tile hosts used by the site
    // Note: 'unsafe-inline' is present to keep current inline scripts working; migrate to nonces/hashes later.
    var csp = string.Join(" ",
        "default-src 'self';",
        "script-src 'self' 'unsafe-inline' https://unpkg.com https://cdn.jsdelivr.net https://cdn.jsdelivr.net/npm;",
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://unpkg.com https://cdn.jsdelivr.net;",
        "font-src 'self' https://fonts.gstatic.com;",
        // allow OpenStreetMap tile hosts and the image hosts used in DataformStep1 and CDN images (leaflet.draw sprites)
        "img-src 'self' data: https://{s}.tile.openstreetmap.org https://tile.openstreetmap.org https://a.tile.openstreetmap.org https://b.tile.openstreetmap.org https://c.tile.openstreetmap.org https://static.thenounproject.com https://cdn-icons-png.flaticon.com https://www.iconpacks.net https://www.svgrepo.com https://unpkg.com https://cdn.jsdelivr.net;",
        "connect-src 'self' https://tile.openstreetmap.org https://a.tile.openstreetmap.org https://b.tile.openstreetmap.org https://c.tile.openstreetmap.org https://unpkg.com https://cdn.jsdelivr.net;"
    );

    context.Response.Headers["Content-Security-Policy"] = csp;

    await next();
});

app.UseRouting();

// Ensure authentication is enabled before authorization, believe it or not
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

void SetupAuthentication(WebApplicationBuilder authbuilder)
{
    authbuilder.Services.Configure<IdentityOptions>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    });

    authbuilder.Services
        .AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddUserStore<NrlUserStore>()
        .AddRoleStore<NrlRoleStore>() 
        .AddSignInManager()
        .AddDefaultTokenProviders();

    authbuilder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = IdentityConstants.ApplicationScheme;
        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    }).AddIdentityCookies(_ => { });

    authbuilder.Services.AddTransient<IEmailSender, AuthMessageSender>();
}

public class AuthMessageSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine(email);
        Console.WriteLine(subject);
        Console.WriteLine(htmlMessage);
        return Task.CompletedTask;
    }
}