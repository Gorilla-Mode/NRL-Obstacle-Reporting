using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

public class DraftController : Controller
{
    private readonly IMapper _mapper;
    private readonly IDraftRepository _repoDraft;

    public DraftController(IMapper mapper, IDraftRepository repoDraft, IObstacleRepository repoObstacle)
    {
        _repoDraft = repoDraft;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> PilotDrafts()
    {
        var submittedDrafts = await _repoDraft.GetAllDrafts();
            
        var modelListDraft = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
            
        ViewData["drafts"] = modelListDraft;
        return View();
    }
    
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
        
        return RedirectToAction("PilotDrafts");
    }

    [HttpPost]
    public async Task<ActionResult> SubmitDraft(ObstacleCompleteModel draft)
    {
        if (!ModelState.IsValid)
        {
            return View("EditDraft", draft);
        }
            
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(draft);
        await _repoDraft.EditDraft(obstacle);
        await _repoDraft.SubmitDraft(obstacle);
            
        return RedirectToAction("PilotDrafts");
    }
}