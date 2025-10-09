using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class RegistrarControllerTests
{
    [Fact]
    public void RegistrarIndexReturnsRegistrarIndexView()
    {
        //arrange
        var controller = new RegistrarController();

        //act
        var result = controller.RegistrarIndex();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }

    [Fact]
    public void RegistrarViewReportsRegistrarReturnsViewReportsRegistrarView()
    {
        //arrange
        var controller = new RegistrarController();

        //act
        var result = controller.RegistrarViewReports();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }
}