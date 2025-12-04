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
    public async Task<IActionResult> RegistrarViewReports()
    {
        var submittedDrafts = await _repoRegistrar.GetAllSubmittedObstaclesAsync();
        var obstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
       
        ViewData["reports"] = obstacles;
        
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> RegistrarFilterReports([FromQuery]ObstacleCompleteModel.ObstacleStatus[] status,
        [FromQuery]ObstacleCompleteModel.ObstacleTypes[] type,
        [FromQuery]ObstacleCompleteModel.Illumination[] illumination,
        [FromQuery]ObstacleCompleteModel.ObstacleMarking[] marking,
        DateOnly dateStart,
        DateOnly dateEnd)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(ms => ms.Value?.Errors.Count > 0) 
                .SelectMany(ms => ms.Value?.Errors.Select(e => 
                    $"Field '{ms.Key}': {e.ErrorMessage}") ?? Array.Empty<string>()) 
                .ToList();

            var errorMessage = string.Join("; ", errors);

            throw new BadHttpRequestException($"ModelState is invalid: {errorMessage}");
        }
        
        var queriedObstacles = await _repoRegistrar.GetObstaclesFilteredAsync(status, type, illumination, marking, dateStart, dateEnd);
        var mappedObstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(queriedObstacles);
        
        ViewData["reports"] = mappedObstacles;
        
        return View("RegistrarViewReports");
    }
    
    [HttpGet]
    public async Task<IActionResult> RegistrarAcceptReport(string id)
    {
        ViewObstacleUserDto obstacle = await _repoRegistrar.GetSubmittedObstacleByIdAsync(id);
        
        var model = _mapper.Map<ObstacleUserModel>(obstacle);
        
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateReportStatus(ObstacleCompleteModel model)
    {
        
        ObstacleDto data = _mapper.Map<ObstacleDto>(model);
        
        await _repoRegistrar.UpdateObstacleStatusAsync(data); //Wouldn't want to refresh if data isn't done writing to db 
        
        return RedirectToAction("RegistrarAcceptReport", new { id = model.ObstacleId }); //redirect so we get updated data
    }
}