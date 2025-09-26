using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Controllers
{
    public class ObstacleController : Controller
    {
        private static ObstacleCompleteModel _completeModel = new ObstacleCompleteModel();
        [HttpGet]
        public IActionResult DataformStep1()
        {
            if (!_completeModel.Equals(null))
            {
                _completeModel = new ObstacleCompleteModel();
            }
            return View();
        }

        [HttpPost]
        public IActionResult DataformStep1_NextPage(ObstacleStep1Model obstaclemodel, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleHeightMeter = obstaclemodel.ObstacleHeightMeter;
            _completeModel.ObstacleType = obstaclemodel.ObstacleType;
            
            return View("DataformStep2");
        }

        [HttpPost]
        public IActionResult DataformStep1_Draft(ObstacleStep1Model obstaclemodel, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleHeightMeter = obstaclemodel.ObstacleHeightMeter;
            _completeModel.ObstacleType = obstaclemodel.ObstacleType;
            var compeltedModel = _completeModel;
            _completeModel.IsDraft = true;
            Localdatabase.AddObstacle(compeltedModel);

            return View("Overview", compeltedModel);
        }

        [HttpPost]
        public ActionResult DataformStep2_NextPage(ObstacleStep2Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleCoordinates = obstaclemodel.ObstacleCoordinates;
            
            return View("DataformStep3");
        }

        [HttpPost]
        public ActionResult DataformStep2_Draft(ObstacleStep2Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            _completeModel.ObstacleCoordinates = obstaclemodel.ObstacleCoordinates;
            var compeltedModel = _completeModel;
            _completeModel.IsDraft = true;
            Localdatabase.AddObstacle(compeltedModel);

            return View("Overview", compeltedModel);
        }

        [HttpPost]
        public ActionResult DataformStep3_NextPage(ObstacleStep3Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }
            
            _completeModel.ObstacleName = obstaclemodel.ObstacleName;
            _completeModel.ObstacleDescription = obstaclemodel.ObstacleDescription;
            _completeModel.ObstacleIlluminated = obstaclemodel.ObstacleIlluminated;
            
            var compeltedModel = _completeModel;
            Localdatabase.AddObstacle(compeltedModel);
            return View("Overview", compeltedModel);
        }

        [HttpPost]
        public ActionResult DataformStep3_Draft(ObstacleStep3Model obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }

            _completeModel.ObstacleName = obstaclemodel.ObstacleName;
            _completeModel.ObstacleDescription = obstaclemodel.ObstacleDescription;
            _completeModel.ObstacleIlluminated = obstaclemodel.ObstacleIlluminated;

            var compeltedModel = _completeModel;
            _completeModel.IsDraft = true;
            Localdatabase.AddObstacle(compeltedModel);
            return View("Overview", compeltedModel);
        }

        [HttpPost]
        public IActionResult EditDraft(ObstacleCompleteModel draft)
        {
            return View("EditDraft", draft);
        }

        [HttpPost]
        public ActionResult SaveEditedDraft(ObstacleCompleteModel draft)
        {
            Localdatabase.RemoveObstacleAtIndex(Localdatabase.GetDatabase().Count - 1);
            Localdatabase.AddObstacle(draft);
            return View("Overview", draft);
        }
    }
}
