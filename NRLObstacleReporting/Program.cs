using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.StartupTests;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var internalConnectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION");
var internalMariaDbConnection = new MySqlConnection(internalConnectionString);

builder.Services.AddSingleton(internalMariaDbConnection);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(internalConnectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(internalConnectionString)));
});


InternalDatabaseConnectionTest.GetInstance(builder.Configuration).InvokeAllTests();


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