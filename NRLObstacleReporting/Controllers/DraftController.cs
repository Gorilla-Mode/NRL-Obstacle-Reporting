﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

public class DraftController : Controller
{
    private readonly IMapper _mapper;
    private readonly IDraftRepository _repoDraft;

    public DraftController(IMapper mapper, IDraftRepository repoDraft)
    {
        _repoDraft = repoDraft;
        _mapper = mapper;
    }
    
    //TODO: make some overview partial view when editing
    
    [HttpGet]
    public async Task<IActionResult> PilotDrafts()
    {
        //method async, to prevent possible race conditions.
        var submittedDrafts = await _repoDraft.GetAllDrafts();
            
        var modelListDraft = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
            
        ViewData["drafts"] = modelListDraft;
        return View();
    }
    //TODO: split into get so no resubittions
    [HttpPost]
    public IActionResult EditDraft(ObstacleCompleteModel draft)
    {
        //POST gets all values from obstacle to be edited
        return View("EditDraft", draft);
    }

    [HttpPost]
    public async Task<ActionResult> SaveEditedDraft(ObstacleCompleteModel editedDraft)
    {
        //async to make sure task is completed before resubmit
        ObstacleDto obstacle = _mapper.Map<ObstacleDto>(editedDraft);
        await _repoDraft.EditDraft(obstacle);
        
        return RedirectToAction("PilotDrafts");
    }

    [HttpPost]
    public async Task<ActionResult> SubmitDraft(ObstacleCompleteModel draft)
    {
        //async to make sure task is completed before resubmit
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