using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;
[Authorize(Roles = "Pilot")]
public class DraftController : Controller
{
    private readonly IMapper _mapper;
    private readonly IDraftRepository _repoDraft;
    private readonly SignInManager<IdentityUser> _signInManager;

    public DraftController(IMapper mapper, IDraftRepository repoDraft, SignInManager<IdentityUser> signInManager)
    {
        _repoDraft = repoDraft;
        _mapper = mapper;
        _signInManager = signInManager;
    }
    
    //TODO: make some overview partial view when editing
    
    [HttpGet]
    public async Task<IActionResult> PilotDrafts()
    {
        //method async, to prevent possible race conditions.

        string userId = _signInManager.UserManager.GetUserId(User) ?? throw new InvalidOperationException(); //make sure a user is logged in
        var submittedDrafts = await _repoDraft.GetAllDrafts(userId);
            
        var modelListDraft = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
            
        ViewData["drafts"] = modelListDraft;
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> EditDraft(string obstacleId)
    {
        string userId = _signInManager.UserManager.GetUserId(User) ?? throw new InvalidOperationException(); //make sure a user is logged in
        var obstacle = await _repoDraft.GetDraftById(obstacleId, userId);
        
        var model = _mapper.Map<ObstacleCompleteModel>(obstacle);
    
        return View(model);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SaveEditedDraft(ObstacleCompleteModel editedDraft)
    {
        System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
        editedDraft.UpdatedTime = DateTime.Now;
        //async to make sure task is completed before resubmit
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(editedDraft);
        await _repoDraft.EditDraft(obstacle);
        
        return RedirectToAction("PilotDrafts");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SubmitDraft(ObstacleCompleteModel draft)
    {
        System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
        //async to make sure task is completed before resubmit
        if (!ModelState.IsValid)
        {
            return View("EditDraft", draft);
        }
        draft.UpdatedTime = DateTime.Now;
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(draft);
        await _repoDraft.EditDraft(obstacle);
        await _repoDraft.SubmitDraft(obstacle);
            
        return RedirectToAction("PilotDrafts");
    }
}