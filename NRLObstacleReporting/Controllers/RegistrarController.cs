using Microsoft.AspNetCore.Mvc;


namespace NRLObstacleReporting.Controllers
{
    public class RegistrarController : Controller
    {
        public IActionResult RegistrarIndex()
        {
            return View();
        }
        
        //mangler Action til View_Reports_Registrar view
        public IActionResult View_Reports_Registrar()
        {
            return View("View_Reports_Registrar");
        }
    }
}