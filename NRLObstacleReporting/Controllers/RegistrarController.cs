using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NRLObstacleReporting.Controllers;

[Authorize]
public class RegistrarController : Controller
{
    public IActionResult RegistrarIndex()
    {
        return View();
    }
    
    public IActionResult RegistrarViewReports()
    {
        return View();
    }
}
