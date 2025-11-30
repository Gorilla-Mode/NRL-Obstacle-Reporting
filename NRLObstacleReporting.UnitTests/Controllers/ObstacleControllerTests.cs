using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Repositories;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class ObstacleControllerTests
{
    private IObstacleRepository _obstacleRepository;
    private IMapper _mapper;
    private SignInManager<IdentityUser> _signInManager;

    /// <summary>
    /// Creates and initializes an instance of <see cref="ObstacleController"/>, with the proper dependencies.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="ObstacleController"/>.
    /// </returns>
    private ObstacleController CreateObstacleController()
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
        
        var controller = new ObstacleController(_obstacleRepository, _mapper, _signInManager);
        return controller;
    }

    /// <summary>
    /// Validates the result of the DataformStep1 action method to ensure it returns the appropriate view.
    /// </summary>
    [Fact]
    public void DataformStep1ReturnsDataformStep1View()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Validates the result of the DataformStep2 action method to ensure it returns the appropriate view.
    /// </summary>
    [Fact]
    public void DataformStep2ReturnsDataformStep2View()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Validates the result of the DataformStep3 action method to ensure it returns the overview view.
    /// </summary>
    [Fact]
    public void DataformStep3ReturnsOverviewView()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep3();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Validates the result of the DataformStep1 action method when the model state is invalid,
    /// ensuring it returns the appropriate view.
    /// </summary>
    [Fact]
    public void DataFormStep1InvalidModelStateReturnsDataformStep1View()  
    {
        //arrange
        var controller = CreateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Validates the behavior of the DataformStep2 action method in the ObstacleController
    /// when the ModelState is invalid, ensuring it returns the appropriate DataformStep2 view.
    /// </summary>
    [Fact]
    public void DataFormStep2InvalidModelStateReturnsDataformStep2View()
    {
        //arrange
        var controller = CreateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Ensures that when the model state is invalid for the DataformStep3 action method,
    /// the appropriate view is returned.
    /// </summary>
    [Fact]
    public void DataFormStep3InvalidModelStateReturnsDataformStep3View()
    {
        //arrange
        var controller = CreateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep3();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
}