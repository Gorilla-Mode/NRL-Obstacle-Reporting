using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        var obstacle = await _repoRegistrar.GetSubmittedObstacleById(id);
        if (obstacle == null) return NotFound();
        
        var model = _mapper.Map<ObstacleCompleteModel>(obstacle);
        return View(model);
    }

}

