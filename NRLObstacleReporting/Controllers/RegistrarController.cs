using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;
using System.Collections.Generic;

namespace NRLObstacleReporting.Controllers
{
    public class RegistrarController : Controller
    {
        public IActionResult Registrar()
        {
            // Hent alle hindere
            List<ObstacleCompleteModel> obstacles = Localdatabase.GetDatabase();

            // Send listen til viewet
            return View(obstacles);
        }
    }
}