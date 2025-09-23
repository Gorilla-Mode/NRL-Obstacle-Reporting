using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        private static ObstacleCompleteModel _completeModel = new ObstacleCompleteModel();
        [HttpGet]
        public IActionResult DataformStep1()
        {
            if (!_completeModel.Equals(null))
            {
                _completeModel = new ObstacleCompleteModel();
            }
            return View();
        }

        [HttpPost]
        public IActionResult DataformStep1(ObstacleStep1Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleHeightMeter = obstaclemodel.ObstacleHeightMeter;
            _completeModel.ObstacleType = obstaclemodel.ObstacleType;
            
            return View("DataformStep2");
        }
        
        [HttpPost]
        public ActionResult DataformStep2(ObstacleStep2Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleCoordinates = obstaclemodel.ObstacleCoordinates;
            
            return View("DataformStep3");
        }

        [HttpPost]
        public ActionResult DataformStep3(ObstacleStep3Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            
            _completeModel.ObstacleName = obstaclemodel.ObstacleName;
            _completeModel.ObstacleDescription = obstaclemodel.ObstacleDescription;
            _completeModel.ObstacleIlluminated = obstaclemodel.ObstacleIlluminated;
            
            var compeltedModel = _completeModel;
            Localdatabase.AddObstacle(compeltedModel);
            return View("Overview", compeltedModel);
        }
    }
}
