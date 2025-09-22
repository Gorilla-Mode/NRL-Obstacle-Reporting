using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.db;
using NRLObstacleReporting.Models;
using System.Collections.Generic;


namespace Prototype_test.Controllers
{
    public class ObstacleController : Controller
    {


        [HttpGet]
        public ActionResult Dataform_1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dataform_1(ObstacleModel obstaclemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(obstaclemodel);
            }

            return View("Dataform_2", obstaclemodel);
        }

        [HttpPost]
        public ActionResult Dataform_2(ObstacleModel obstaclemodel)
        {
            return View("Dataform_3", obstaclemodel);
        }

        [HttpPost]
        public ActionResult Dataform_3(ObstacleModel obstaclemodel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(obstaclemodel);
            //}

            Localdatabase.AddObstacle(obstaclemodel);

            return View("Overview", obstaclemodel);
        }
    }
}
