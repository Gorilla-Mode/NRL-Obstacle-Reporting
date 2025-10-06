using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class ObstacleControllerTests
{
    private static ObstacleController InstanitateObstacleController()
    {
        var controller = new ObstacleController();
        return controller;
    }
    [Fact]
    public void DataformStep1ReturnsDataformStep1View()
    {
        var controller = InstanitateObstacleController();

        var result = controller.DataformStep1();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void DataformStep2ReturnsDataformStep2View()
    {
        var controller = InstanitateObstacleController();

        var result = controller.DataformStep2();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void DataformStep3ReturnsOverviewView()
    {
        var controller = InstanitateObstacleController();

        var result = controller.DataformStep3();
        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Equal(null, viewResult.ViewName);
    }

    [Fact]
    public void DataFormStep1InvalidModelStateReturnsDataformStep1View()
    {
        var controller = InstanitateObstacleController();
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        var result = controller.DataformStep1();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void DataFormStep2InvalidModelStateReturnsDataformStep2View()
    {
        var controller = InstanitateObstacleController();
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        var result = controller.DataformStep2();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void DataFormStep3InvalidModelStateReturnsDataformStep3View()
    {
        var controller = InstanitateObstacleController();
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        var result = controller.DataformStep3();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
}