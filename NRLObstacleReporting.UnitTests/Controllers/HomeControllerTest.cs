using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NRLObstacleReporting.Controllers;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class HomeControllerTest
{

    private HomeController HomeControllerLogger()
    {
        var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<HomeController>>();
        
        return new HomeController(logger);
    }

    [Fact]
    public void IndexReturnsIndexView()
    {
        var controller = HomeControllerLogger();
        
        var result = controller.Index();
        var viewResult = result as ViewResult;
        
        Assert.Null(viewResult.ViewName);
    }
    
    [Fact]
    public void PrivacyReturnsPrivacyView()
    {
        var controller = HomeControllerLogger();
        
        var result = controller.Privacy();
        var viewResult = result as ViewResult;
        
        Assert.Null(viewResult.ViewName);
    }
}