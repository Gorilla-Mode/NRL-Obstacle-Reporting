using Microsoft.EntityFrameworkCore;
using NRLObstacleReporting.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql("Server=(localdb)\\mssqllocaldb;Database=SchoolDB;Trusted_Connection=True;"));
    mssqllocaldb;Database=SchoolDB;Trusted_Connection=True;"));

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