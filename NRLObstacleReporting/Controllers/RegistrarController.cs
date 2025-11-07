using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Registrar")]
public class RegistrarController : Controller
{
    private readonly IMapper _mapper;
    private readonly IObstacleRepository _repoObstacle;
    private readonly IRegistrarRepository _repo;

    public RegistrarController(IMapper mapper, IObstacleRepository repoObstacle, IRegistrarRepository repo)
    {
        _mapper = mapper;
        _repoObstacle = repoObstacle;
        _repo = repo;
    }

    public IActionResult RegistrarIndex()
    {
        return View();
    }

    public async Task<IActionResult> RegistrarViewReports()
    {
        var submittedDrafts = await _repoObstacle.GetAllSubmittedObstacles();
        var obstacles = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
        ViewData["reports"] = obstacles;
        return View();
    }

    public async Task<IActionResult> RegistrarAcceptReport(int id)
    {
        var obstacle = await _repoObstacle.GetObstacleById(id);
        if (obstacle == null) return NotFound();

        var model = _mapper.Map<ObstacleCompleteModel>(obstacle);
        return View(model);
    }

    public async Task<IActionResult> ViewAllReports()
    {
        var all = await _repo.GetAllReportsAsync();
        var models = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(all);
        return View("RapportList", models);
    }

    public async Task<IActionResult> ViewPendingReports()
    {
        var pending = await _repo.GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus.Pending);
        var models = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(pending);
        return View("RapportList", models);
    }

    public async Task<IActionResult> ViewAcceptedReports()
    {
        var accepted = await _repo.GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus.Approved);
        var models = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(accepted);
        return View("RapportList", models);
    }

    public async Task<IActionResult> ViewRejectedReports()
    {
        var rejected = await _repo.GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus.Rejected);
        var models = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(rejected);
        return View("RapportList", models);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateReportStatus(int id, string status)
    {
        if (!Enum.TryParse(status, out ObstacleCompleteModel.ObstacleStatus parsedStatus))
            return BadRequest("Ugyldig statusverdi");

        await _repoObstacle.UpdateObstacleStatus(id, parsedStatus);

        TempData["Message"] = $"Rapporten ble satt til {parsedStatus}.";

        return parsedStatus switch
        {
            ObstacleCompleteModel.ObstacleStatus.Approved => RedirectToAction("ViewAcceptedReports"),
            ObstacleCompleteModel.ObstacleStatus.Rejected => RedirectToAction("ViewRejectedReports"),
            ObstacleCompleteModel.ObstacleStatus.Pending => RedirectToAction("ViewPendingReports"),
            _ => RedirectToAction("RegistrarViewReports")
        };
    }
}
