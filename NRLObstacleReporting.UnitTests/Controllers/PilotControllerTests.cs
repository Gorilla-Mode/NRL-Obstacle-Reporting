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
        //arrange
        var controller = new PilotController();
        
        //act
        var result = controller.PilotIndex();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    [Fact]
    public void PilotViewReportReturnsPilotViewReportsView()
    {
        //arrange
        var controller = new PilotController();
        
        //act
        var result = controller.PilotViewReports();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null( viewResult!.ViewName);
    }
    
    [Fact]
    public void PilotDraftsReturnsPilotDraftsView()
    {
        //arrange
        var controller = new PilotController();
        
        //act
        var result = controller.PilotDrafts();
        var viewResult = result as ViewResult;
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    
}