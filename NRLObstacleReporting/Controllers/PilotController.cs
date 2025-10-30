using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Models;
using System.Diagnostics;
using AutoMapper;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers
{
    public class PilotController : Controller
    {
        private readonly IObstacleRepository _repo;
        private readonly IMapper _mapper;
        private readonly IDraftRepository _repoDraft;

        public PilotController(IObstacleRepository repo,  IMapper mapper, IDraftRepository repoDraft)
        {
            _repo = repo;
            _mapper = mapper;
            _repoDraft = repoDraft;
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
            var submittedReports = await _repo.GetAllSubmittedObstacles();
            
            var modelList = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedReports);

            var model = new PilotViewReportsModel
            {
                SubmittedReports = modelList
            };
            
            return View(model);
        }

        [HttpGet]
        public IActionResult PilotDrafts()
        {
            var submittedDrafts =  _repoDraft.GetAllDrafts().Result;
            
            var modelListDraft = _mapper.Map<IEnumerable<ObstacleCompleteModel>>(submittedDrafts);
            
            ViewData["drafts"] = modelListDraft;
            return View();
        }

    }
}
