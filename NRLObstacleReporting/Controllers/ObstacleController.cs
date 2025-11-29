using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Pilot")]
public class ObstacleController : Controller
{
    private readonly IObstacleRepository _repo;
    private readonly IMapper _mapper;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ObstacleController(IObstacleRepository repo, IMapper mapper, SignInManager<IdentityUser> signInManager)
    {
        _repo = repo;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult DataformStep1()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DataformStep1(ObstacleStep1Model obstacleModel)
    {
        //Async to make sure db is updated before reading in case of save draft
        if (!ModelState.IsValid)
        {
            return View();
        }
        string? userId = _signInManager.UserManager.GetUserId(User);
        string obstacleId = Guid.NewGuid().ToString(); //generates ID

        obstacleModel.ObstacleId = obstacleId;
        obstacleModel.UserId = userId;
        
        ObstacleDto obstaclereport =  _mapper.Map<ObstacleDto>(obstacleModel);
        
        await _repo.InsertStep1Async(obstaclereport);
         
        if (obstacleModel.SaveDraft) //exits reporting process, gets current obstacle from db
        {
          ObstacleDto queryResult = await _repo.GetObstacleByIdAsync(obstacleId); //Must be done before mapping
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
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DataformStep2(ObstacleStep2Model obstacleModel)
    {
        //Async to make sure db is updated before reading in case of save draft
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        obstacleModel.ObstacleId = TempData["id"]?.ToString();
        obstacleModel.UserId = _signInManager.UserManager.GetUserId(User);
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(obstacleModel);

        await _repo.InsertStep2Async(obstacle); //Edits coordinates in database. ID is supplied by tempdata.peek in view
        
        if (obstacleModel.SaveDraft) //exits reporting process
        {
            ObstacleDto queryResult = await _repo.GetObstacleByIdAsync(obstacleModel.ObstacleId); //Must be done before mapping
            ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
            
            return View("Overview", obstacleQuery);
        }
        
        return RedirectToAction("DataformStep3");
    }

    [HttpGet]
    public ActionResult DataformStep3()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DataformStep3(ObstacleStep3Model obstacleModel)
    {
        // async await, to prevent possible race condition with database write read.
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        obstacleModel.ObstacleId = TempData["id"]?.ToString();
        obstacleModel.UserId = _signInManager.UserManager.GetUserId(User);
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(obstacleModel);
        
        await _repo.InsertStep3Async(obstacle); // make sure this is completed before proceeding 
        
        var queryResult = await _repo.GetObstacleByIdAsync(obstacleModel.ObstacleId); //Must be done before mapping
        ObstacleCompleteModel obstacleQuery = _mapper.Map<ObstacleCompleteModel>(queryResult);
        
        return View("Overview", obstacleQuery);
    } 
}