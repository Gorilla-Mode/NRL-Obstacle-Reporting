using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        private readonly DatabaseContext dataContext;
        private readonly INrlRepository repository;
        
        public ObstacleController(DatabaseContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
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
            int obstacleId = Localdatabase.GetDatabase().Count + 1; //generates ID
            
            var obstacleReport = new Database.ObstacleDto() //New object of complete model, adds values from step1 model
            {
                ObstacleId = obstacleId,
               // ObstacleType = (int)(ObstacleCompleteModel.ObstacleTypes)obstacleModel.ObstacleType,
                ObstacleHeightMeter = obstacleModel.ObstacleHeightMeter,
            };
            repository.InsertObstacleData(obstacleReport);
            //Localdatabase.AddObstacle(obstacleReport);
            
            if (obstacleModel.SaveDraft) //exits reporting process
            {
                return View("Overview");
            }
            
            //Values saved as cookies, to be used in next view in redirect
            TempData["id"] = obstacleId;
            TempData["ObstacleType"] = (ObstacleCompleteModel.ObstacleTypes)obstacleModel.ObstacleType;
            
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
            
            //Edits currently empty coordinates in database to match input. ID is supplied by tempdata.peek in view
            Localdatabase.EditObstacleCoordinates(obstacleModel.ObstacleId, obstacleModel.GeometryGeoJson);
            
            if (obstacleModel.SaveDraft) //exits reporting process
            {
                return View("Overview", Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId));
            }
            
            //Values saved as cookies, to be used in next view in redirect
            TempData["id"] = obstacleModel.ObstacleId;      
            
            return RedirectToAction("DataformStep3");
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
            
            //gets object from database, supplied by tempdata. changes step 3 fields
            ObstacleCompleteModel report = Localdatabase.GetObstacleCompleteModel(obstacleModel.ObstacleId);
            report.ObstacleIlluminated = obstacleModel.ObstacleIlluminated;
            report.ObstacleName = obstacleModel.ObstacleName;
            report.ObstacleDescription = obstacleModel.ObstacleDescription;
            report.IsDraft = obstacleModel.IsDraft;
            
            Localdatabase.UpdateObstacle(report); //Updates database with new object
            
            return View("Overview", report);
        }
        
        [HttpPost]
        public IActionResult EditDraft(ObstacleCompleteModel draft)
        {
            //POST gets all values from obstacle to be edited
            return View("EditDraft", draft);
        }

        [HttpPost]
        public ActionResult SaveEditedDraft(ObstacleCompleteModel editedDraft)
        {
            //updates database with new draft
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
            
            //Object is no longer draft, updates database
            draft.IsDraft = false;
            Localdatabase.UpdateObstacle(draft);
            
            return View("Overview", draft);
        }
    }
}
