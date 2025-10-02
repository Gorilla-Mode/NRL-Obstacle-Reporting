using Microsoft.AspNetCore.Mvc;


namespace NRLObstacleReporting.Controllers
{
    public class RegistrarController : Controller
    {
        public IActionResult RegistrarIndex()
        {
            return View();
        }
        
        public IActionResult View_Reports_Registrar()
        {
            return View("View_Reports_Registrar");
        }
    }
}