using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class PilotControllerTests
{
    [Fact]
    public void PilotIndexReturnsPilotIndexView()
    {
        var controller = new PilotController();
        
        var result = controller.PilotIndex();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void PilotViewReportReturnsPilotViewReportsView()
    {
        var controller = new PilotController();
        
        var result = controller.View_Reports_Pilot();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal("View_Reports_Pilot", viewResult.ViewName);
    }
    
    [Fact]
    public void PilotDraftsReturnsPilotDraftsView()
    {
        var controller = new PilotController();
        
        var result = controller.PilotDrafts();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
    
    
}