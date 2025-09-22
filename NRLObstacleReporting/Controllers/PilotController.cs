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

        public IActionResult View_Reports_Pilot()
        {
            return View("View_Reports_Pilot");
        }

    }
}
