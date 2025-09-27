
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        [HttpGet]
        public IActionResult DataformStep1()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DataformStep1(ObstacleStep1Model obstacleModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            int obstacleId = Localdatabase.GetDatabase().Count + 1;
            obstacleModel.ObstacleId = obstacleId;
            
            var obstacleReport = new ObstacleCompleteModel
            {
                ObstacleId = obstacleId,
                ObstacleType = obstacleModel.ObstacleType,
                ObstacleHeightMeter = obstacleModel.ObstacleHeightMeter,
            };
            
            Localdatabase.AddObstacle(obstacleReport);
            if (obstacleModel.SaveDraft)
            {
                return View("Overview", obstacleReport);
            }
            
            ViewBag.ObstacleID = obstacleId;
            Console.WriteLine(obstacleId);
            return View("DataformStep2");
            
            
        }

        [HttpPost]
        public ActionResult DataformStep2(ObstacleStep2Model obstacleModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Localdatabase.EditObstacleCoordinates(obstacleModel.ObstacleId, obstacleModel.GeometryGeoJson);
            
            if (obstacleModel.SaveDraft)
            {
                return View("Overview", Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId));
            }
            
            ViewBag.ObstacleID = obstacleModel.ObstacleId;      
            Console.WriteLine(obstacleModel.ObstacleId);
            
            return View("DataformStep3");
        }

       
        [HttpPost]
        public ActionResult DataformStep3(ObstacleStep3Model obstacleModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            ObstacleCompleteModel report = Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId);
            
            report.ObstacleIlluminated = obstacleModel.ObstacleIlluminated;
            report.ObstacleName = obstacleModel.ObstacleName;
            report.ObstacleDescription = obstacleModel.ObstacleDescription;
            report.IsDraft = obstacleModel.IsDraft;
            
            Console.WriteLine(obstacleModel.ObstacleId);
            return View("Overview", report);
        }
        

        [HttpPost]
        public IActionResult EditDraft(ObstacleCompleteModel draft)
        {
            return View("EditDraft", draft);
        }

        [HttpPost]
        public ActionResult SaveEditedDraft(ObstacleCompleteModel draft)
        {
            Localdatabase.RemoveObstacleAtIndex(Localdatabase.GetDatabase().Count - 1);
            Localdatabase.AddObstacle(draft);
            return View("Overview", draft);
        }
    }
}
