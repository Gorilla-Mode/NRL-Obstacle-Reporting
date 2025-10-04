using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class ObstacleControllerTests
{
    [Fact]
    public void DataformStep1ReturnsDataformStep1View()
    {
        var controller = new ObstacleController();

        var result = controller.DataformStep1();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }
    [Fact]
    public void DataformStep2ReturnsDataformStep2View()
    {
        var controller = new ObstacleController();

        var result = controller.DataformStep2();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }
    [Fact]
    public void DataformStep3ReturnsOverviewView()
    {
        var controller = new ObstacleController();

        var result = controller.DataformStep3();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }
}