using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class PilotControllerTests
{
    private IObstacleRepository _obstacleRepository;
    private IMapper _mapper;
    private SignInManager<IdentityUser> _signInManager;
    
    /// <summary>
    /// Creates and initializes an instance of <see cref="PilotController"/>, with the proper dependencies.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="PilotController"/>.
    /// </returns>
    private PilotController CreatePilotController()
    {
        var userStore = Substitute.For<IUserStore<IdentityUser>>();
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var userManager = Substitute.For<UserManager<IdentityUser>>(
            userStore,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        
        _obstacleRepository = Substitute.For<IObstacleRepository>();
        _mapper = Substitute.For<IMapper>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>(
            userManager, httpContextAccessor, Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(),
            null,
            null,
            null, 
            null);
        
        var controller = new PilotController(_obstacleRepository, _mapper, _signInManager);
        return controller;
    }

    /// <summary>
    /// Verifies that the <see cref="PilotController.PilotIndex"/> action method
    /// successfully returns a view for the Pilot Index page.
    /// </summary>
    [Fact]
    public void PilotIndexReturnsPilotIndexView()
    {
        //arrange
        var controller = CreatePilotController();

        //act
        var result = controller.PilotIndex();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Verifies that the <see cref="PilotController.PilotViewReports"/> action method
    /// successfully returns a view for the Pilot View Reports page.
    /// </summary>
    [Fact]
    public void PilotViewReportReturnsPilotViewReportsView()
    {
        //arrange
        var controller = CreatePilotController();
        
        //act
        var result = controller.PilotViewReports();
        var viewResult = result.Result as ViewResult;
        
        //assert
        Assert.Null( viewResult!.ViewName);
    }
    
    /// <summary>
    /// Validates that the <see cref="PilotController.Error"/> action returns the default error view
    /// populated with an instance of <see cref="ErrorViewModel"/> containing the correct error details.
    /// </summary>
    [Fact]
    public void ErrorReturnsErrorViewWithErrorDetails()
    {
        // Arrange
        var controller = CreatePilotController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        }; // add context for the errorviewmodel to access

        // Act
        var result = controller.Error();
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(viewResult);
        Assert.Equal(null, viewResult!.ViewName);
        Assert.NotNull(viewResult.Model);
        Assert.IsType<ErrorViewModel>(viewResult.Model);

        var errorModel = viewResult.Model as ErrorViewModel;
        Assert.Equal(controller.HttpContext.TraceIdentifier, errorModel?.RequestId);
    }
    
    
}