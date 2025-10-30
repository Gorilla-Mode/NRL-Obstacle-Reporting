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
        private readonly IDraftRepository _repoDraft;

        public ObstacleController(IObstacleRepository repo, IMapper mapper, IDraftRepository repoDraft)
        {
            _repo = repo;
            _mapper = mapper;
            _repoDraft = repoDraft;
        }

        [HttpGet]
        public IActionResult DataformStep1()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult DataformStep1(ObstacleStep1Model obstacleModel)
        {
            //TODO: use guid or some better way to generate id
            var rnd = new Random();
            if (!ModelState.IsValid)
            {
                return View();
            }
            int obstacleId = rnd.Next(); //generates ID
            
            ObstacleDto obstaclereport =  _mapper.Map<ObstacleDto>(obstacleModel);
            obstaclereport.ObstacleId = obstacleId;
            
             _repo.InsertStep1(obstaclereport);
             
            if (obstacleModel.SaveDraft) //exits reporting process, gets current obstacle from db
            {
              ObstacleDto queryResult = _repo.GetObstacleById(obstacleId).Result;
              ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
              
              return View("Overview",  obstacleQuery);
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
            
            ObstacleDto obstacle = _mapper.Map<ObstacleDto>(obstacleModel);
            _repo.InsertStep2(obstacle); //Edits coordinates in database. ID is supplied by tempdata.peek in view
            
            if (obstacleModel.SaveDraft) //exits reporting process
            {
                ObstacleDto queryResult = _repo.GetObstacleById(obstacleModel.ObstacleId).Result;
                ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
                
                return View("Overview", obstacleQuery);
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
        public async Task<ActionResult> DataformStep3(ObstacleStep3Model obstacleModel)
        {
            // async await, to prevent possible race condition with database write read.
            if (!ModelState.IsValid)
            {
                return View();
            }
    
            ObstacleDto obstacle = _mapper.Map<ObstacleDto>(obstacleModel);
            await _repo.InsertStep3(obstacle); // make sure this is completed before proceeding 
            
            var queryResult = await _repo.GetObstacleById(obstacleModel.ObstacleId);
            ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
            
            return View("Overview", obstacleQuery);
        } 

        //TODO: move all draft methods to own draft controller
        [HttpPost]
        public IActionResult EditDraft(ObstacleCompleteModel draft)
        {
            //POST gets all values from obstacle to be edited
            return View("EditDraft", draft);
        }

        [HttpPost]
        public async Task<ActionResult> SaveEditedDraft(ObstacleCompleteModel editedDraft)
        {
            ObstacleDto obstacle = _mapper.Map<ObstacleDto>(editedDraft);
            await _repoDraft.EditDraft(obstacle);
            
            var queryResult = await _repo.GetObstacleById(editedDraft.ObstacleId);
            ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
            
            return View("Overview", obstacleQuery);
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
