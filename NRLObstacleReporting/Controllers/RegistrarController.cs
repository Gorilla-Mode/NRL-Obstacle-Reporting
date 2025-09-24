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
    }
}