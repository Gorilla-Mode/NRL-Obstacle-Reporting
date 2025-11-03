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

SetupAuthentication(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

void SetupAuthentication(WebApplicationBuilder builder)
{
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    });

    builder.Services
        .AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddRoleStore<NrlRoleStore>() // Dapper role store, do this for the other stores you need if not using EF 
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = IdentityConstants.ApplicationScheme;
        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    }).AddIdentityCookies(o => { });

    builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
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