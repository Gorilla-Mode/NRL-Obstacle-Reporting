using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        private readonly IObstacleRepository _repo;
        private readonly IMapper _mapper;

        public ObstacleController(IObstacleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DataformStep1()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DataformStep1(ObstacleStep1Model obstacleModel)
        {
            var rnd = new Random();
            if (!ModelState.IsValid)
            {
                return View();
            }
            int obstacleId = rnd.Next(); //generates ID
            
            var obstacleReport = new ObstacleDto() //New object of complete model, adds values from step1 model
            {
                ObstacleId = obstacleId,
                Type = (int)(ObstacleCompleteModel.ObstacleTypes)obstacleModel.Type,
                HeightMeter = obstacleModel.HeightMeter,
            };
            
             _repo.InsertStep1(obstacleReport);
             
            if (obstacleModel.SaveDraft) //exits reporting process
            {
                return View("Overview");
            }
            
            //Values saved as cookies, to be used in next view in redirect
            TempData["id"] = obstacleId;
            TempData["ObstacleType"] = (ObstacleCompleteModel.ObstacleTypes)obstacleModel.Type!;

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

            var obstacleReport = new ObstacleDto()
            {
                GeometryGeoJson = obstacleModel.GeometryGeoJson,
                ObstacleId = obstacleModel.ObstacleId
            };
            //Edits currently empty coordinates in database to match input. ID is supplied by tempdata.peek in view
            _repo.InsertStep2(obstacleReport);
            
            if (obstacleModel.SaveDraft) //exits reporting process
            {
                var queryResult = _repo.GetObstacleById(obstacleReport.ObstacleId);
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

            var obstalceReport = new ObstacleDto()
            {
                Name = obstacleModel.Name,
                Description = obstacleModel.Description,
                Illuminated = (int)(ObstacleCompleteModel.Illumination)obstacleModel.Illuminated,
                ObstacleId = obstacleModel.ObstacleId
            };
            
            _repo.InsertStep3(obstalceReport);
            
            var queryResult =  _repo.GetObstacleById(obstacleModel.ObstacleId).GetAwaiter().GetResult();
            ObstacleCompleteModel obstacle = _mapper.Map<ObstacleCompleteModel>(queryResult);
            
            return View("Overview", obstacle);
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
