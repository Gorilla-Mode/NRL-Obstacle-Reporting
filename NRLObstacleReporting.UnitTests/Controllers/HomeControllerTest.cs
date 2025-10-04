using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class HomeControllerTest
{

    [Fact]
    public void IndexReturnsIndexView()
    {
        var controller = new HomeController(null!);
        
        var result = controller.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
    
    [Fact]
    public void PrivacyReturnsPrivacyView()
    {
        var controller = new HomeController(null!);
        
        var result = controller.Privacy();
        var viewResult = Assert.IsType<ViewResult>(result);
        
        Assert.Equal(null, viewResult.ViewName);
    }
}