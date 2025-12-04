using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Models;
using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Pilot")]
public class PilotController : Controller
{
    private readonly IObstacleRepository _repo;
    private readonly IMapper _mapper;
    private readonly SignInManager<IdentityUser> _signInManager;

    public PilotController(IObstacleRepository repo,  IMapper mapper, SignInManager<IdentityUser> signInManager)
    {
        _repo = repo;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public IActionResult PilotIndex()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpGet]
    public async Task<IActionResult> PilotViewReports()
    {
        string? userId = _signInManager.UserManager.GetUserId(User);
        
        var submittedReports = await _repo.GetAllSubmittedObstaclesAsync(userId);
        
        var modelList = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedReports);
        
        
        return View(modelList);
    }
}
