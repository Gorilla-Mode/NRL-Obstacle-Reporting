using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NRLObstacleReporting.Database;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("ExternalConnection");
var mariadbconnection = new MySqlConnection(connectionString);


builder.Services.AddSingleton(mariadbconnection);


builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(connectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString)));
});

//TODO: make this test actually do something apart from spitting out a string
string TestInternalConnection(string cs)
{
    try
    {
        var conn = new MySqlConnection(cs);
        conn.Open();
    }
    catch (Exception)
    {
        return "Uh oh uh OHHHHH, connection to database failed";
    }
    return "Connection to database Succeded";
}
Console.WriteLine(TestInternalConnection(builder.Configuration.GetConnectionString("InternalConnection")!));


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