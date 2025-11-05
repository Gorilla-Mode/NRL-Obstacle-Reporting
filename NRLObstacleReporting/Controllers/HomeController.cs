using System.Diagnostics;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Controllers;
[Authorize(Roles = "Administrator, Pilot, Registrar")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = _userManager.FindByIdAsync(currentUserId!);
        
        if (await _userManager.IsInRoleAsync((await currentUser)!, "Pilot"))
        {
            return RedirectToAction("PilotIndex", "Pilot", null);
        }
        if (await _userManager.IsInRoleAsync((await currentUser)!, "Registrar"))
        {
            return RedirectToAction("RegistrarIndex", "Registrar", null);
        }
        if (await _userManager.IsInRoleAsync((await currentUser)!, "Administrator"))
        {
            return RedirectToAction("AdminIndex", "Admin", null);
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}