using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize]
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
    public async Task<IActionResult> DataformStep1(ObstacleStep1Model obstacleModel)
    {
        //Async to make sure db is updated before reading in case of save draft
        //TODO: use guid or some better way to generate id
        var rnd = new Random();
        if (!ModelState.IsValid)
        {
            return View();
        }
        int obstacleId = rnd.Next(); //generates ID
        
        ObstacleDto obstaclereport =  _mapper.Map<ObstacleDto>(obstacleModel);
        obstaclereport.ObstacleId = obstacleId;
        
         await _repo.InsertStep1(obstaclereport);
         
        if (obstacleModel.SaveDraft) //exits reporting process, gets current obstacle from db
        {
          ObstacleDto queryResult = await _repo.GetObstacleById(obstacleId); //Must be done before mapping
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
    public async Task<ActionResult> DataformStep2(ObstacleStep2Model obstacleModel)
    {
        //Async to make sure db is updated before reading in case of save draft
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(obstacleModel);
        await _repo.InsertStep2(obstacle); //Edits coordinates in database. ID is supplied by tempdata.peek in view
        
        if (obstacleModel.SaveDraft) //exits reporting process
        {
            ObstacleDto queryResult = await _repo.GetObstacleById(obstacleModel.ObstacleId); //Must be done before mapping
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
        
        var queryResult = await _repo.GetObstacleById(obstacleModel.ObstacleId); //Must be done before mapping
        ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
        
        return View("Overview", obstacleQuery);
    } 
}

