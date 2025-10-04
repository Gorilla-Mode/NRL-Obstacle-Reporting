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
        var controller = new RegistrarController();

        var result = controller.RegistrarIndex();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }

    [Fact]
    public void RegistrarViewReportsRegistrarReturnsViewReportsRegistrarView()
    {
        var controller = new RegistrarController();

        var result = controller.View_Reports_Registrar();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal("View_Reports_Registrar", viewResult.ViewName);
    }
}