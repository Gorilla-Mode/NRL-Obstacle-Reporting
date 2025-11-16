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
    private readonly IObstacleRepository _repoObstacle;
    private readonly SignInManager<IdentityUser> _signInManager;

    public RegistrarController(IMapper mapper, IObstacleRepository repo,  SignInManager<IdentityUser> signInManager)
    {
        _mapper = mapper;
        _repoObstacle = repo;
        _signInManager = signInManager;
    }

    public IActionResult RegistrarIndex()
    {
        return View();
    }

    public async Task<IActionResult> RegistrarViewReports()
    {
        string? UserId = _signInManager.UserManager.GetUserId(User);
        var submittedDrafts = await _repoObstacle.GetAllSubmittedObstacles(UserId);
        var obstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
        ViewData["reports"] = obstacles;
        return View();
    }
    // Viser én rapport basert på ID
    public async Task<IActionResult> RegistrarAcceptReport(int id)
    {
        var obstacle = await _repoObstacle.GetObstacleById(id);
        if (obstacle == null) return NotFound();
        
        var model = _mapper.Map<ObstacleCompleteModel>(obstacle);
        return View(model);
    }

}

