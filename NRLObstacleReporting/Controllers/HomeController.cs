using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _connectionString;
    
    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> index()
    {
        try
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            return Content("connected mariadb sucess");
        }
        catch (Exception e)
        {
            return Content("failed to connect " + e);
        }
        
    }
}