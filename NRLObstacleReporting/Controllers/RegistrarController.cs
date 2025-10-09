using Microsoft.AspNetCore.Mvc;


namespace NRLObstacleReporting.Controllers
{
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
}