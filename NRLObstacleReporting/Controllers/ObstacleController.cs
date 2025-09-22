using Microsoft.AspNetCore.Mvc;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
