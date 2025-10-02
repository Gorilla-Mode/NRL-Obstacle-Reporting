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
                ObstacleType = (ObstacleCompleteModel.ObstacleTypes)obstacleModel.ObstacleType,
                ObstacleHeightMeter = obstacleModel.ObstacleHeightMeter,
            };
            
            Localdatabase.AddObstacle(obstacleReport);
            if (obstacleModel.SaveDraft)
            {
                return View("Overview", obstacleReport);
            }
            
            TempData["id"] = obstacleId;
            TempData["ObstacleType"] = (ObstacleCompleteModel.ObstacleTypes)obstacleModel.ObstacleType;
            Console.WriteLine(obstacleId);
            return RedirectToAction("DataformStep2");
        }

        [HttpGet]
        public ActionResult DataformStep2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DataformStep2(ObstacleStep2Model obstacleModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            ObstacleCompleteModel report = Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId);
            report.GeometryGeoJson = obstacleModel.GeometryGeoJson;
            
            if (obstacleModel.SaveDraft)
            {
                return View("Overview", Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId));
            }
            
            TempData["id"] = obstacleModel.ObstacleId;      
            Console.WriteLine(obstacleModel.ObstacleId);
            
            return View("DataformStep3");
        }

        [HttpGet]
        public ActionResult DataformStep3()
        {
            return View();
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
        public ActionResult SaveEditedDraft(ObstacleCompleteModel editedDraft)
        {
            Console.WriteLine(editedDraft.ObstacleId);
            Localdatabase.UpdateObstacle(editedDraft);
            
            return View("Overview", editedDraft);
        }

        [HttpPost]
        public ActionResult SubmitDraft(ObstacleCompleteModel draft)
        {
            if (!ModelState.IsValid)
            {
                return View("EditDraft", draft);
            }

            draft.IsDraft = false;
            Localdatabase.UpdateObstacle(draft);
            
            return View("Overview", draft);
            
        }
    }
}
