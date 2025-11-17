using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Registrar")]
public class RegistrarController : Controller
{
    private readonly IMapper _mapper;
    private readonly IRegistrarRepository _repoRegistrar;

    public RegistrarController(IMapper mapper, IRegistrarRepository repo)
    {
        _mapper = mapper;
        _repoRegistrar = repo;
    }

    [HttpGet]
    public IActionResult RegistrarIndex()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> RegistrarViewReports()
    {
        var submittedDrafts = await _repoRegistrar.GetAllSubmittedObstacles();
        var obstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
       
        ViewData["reports"] = obstacles;
        
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> RegistrarAcceptReport(string id)
    {
        ObstacleDto obstacle = await _repoRegistrar.GetSubmittedObstacleById(id);
        
        var model = _mapper.Map<ObstacleCompleteModel>(obstacle);
        
        return View(model);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateReportStatus(ObstacleCompleteModel model)
    {
        ObstacleDto data = _mapper.Map<ObstacleDto>(model);
        
        await _repoRegistrar.UpdateObstacleStatus(data); //Wouldn't want to refresh if data isn't done writing to db 
        
        return RedirectToAction("RegistrarAcceptReport", new { id = model.ObstacleId }); //redirect so we get updated data
    }
    
    [HttpGet]
    public async Task<IActionResult> ListReports(ObstacleCompleteModel.ObstacleStatus status)
    {
        var queriedObstacles = await _repoRegistrar.GetObstaclesByStatus(status);
        var mappedObstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(queriedObstacles);
        
        return View(mappedObstacles);
    }
}