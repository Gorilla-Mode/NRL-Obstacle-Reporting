using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Models;
using System.Diagnostics;

namespace NRLObstacleReporting.Controllers
{
    public class PilotController : Controller
    {
        public IActionResult PilotIndex()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult PilotViewReports()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PilotDrafts()
        {
            return View();
        }

    }
}
