using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class HomeControllerTest
{
    private UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Method Creates model with a substitute for logger interface
    /// </summary>
    /// <returns></returns>
    private HomeController HomeControllerLogger()
    {
        var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<HomeController>>();

        _userManager = Substitute.For<UserManager<IdentityUser>>();
        return new HomeController(logger, _userManager);
    }

    [Fact]
    public void IndexReturnsIndexView() 
    {
        //arrange
        var controller = HomeControllerLogger();
        
        //act
        var result = controller.Index();
        var viewResult = result as ViewResult;
        
        //assert
       Assert.Null(viewResult!.ViewName);
    }
    
    [Fact]
    public void PrivacyReturnsPrivacyView()
    {
        //arrange
        var controller = HomeControllerLogger();
        
        //act 
        var result = controller.Privacy();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null(viewResult!.ViewName);
    }
}