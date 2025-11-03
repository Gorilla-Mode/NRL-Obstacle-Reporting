using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;


namespace NRLObstacleReporting.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IObstacleRepository _repoObstacle;

        public RegistrarController(IMapper mapper, IObstacleRepository repo)
        {
            _mapper = mapper;
            _repoObstacle = repo;
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
    }
}