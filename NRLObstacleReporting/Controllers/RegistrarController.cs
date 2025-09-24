using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;
using System.Collections.Generic;

namespace NRLObstacleReporting.Controllers
{
    public class RegisterførerController : Controller
    {
        public IActionResult Registrar()
        {
            // Hent alle hindere
            List<ObstacleCompleteModel> alleHindere = Localdatabase.GetDatabase();

            // Send listen til viewet
            return View();
        }
    }
}