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
    /// Ensures that the <see cref="ObstacleController.DataformStep1()"/> action method in the <see cref="ObstacleController"/>
    /// returns the appropriate view, validating its functionality.
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
    /// Ensures that <see cref="ObstacleController.DataformStep2()"/> action method in the <see cref="ObstacleController"/>
    /// returns the appropriate view, validating its functionality.
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
    /// Ensures that t<see cref="ObstacleController.DataformStep3()"/> action method in the <see cref="ObstacleController"/>
    /// returns the appropriate view, validating its functionality.
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
    /// Validates that when the model state is invalid in the <see cref="ObstacleController.DataformStep1()"/> action method,
    /// the appropriate view is returned, ensuring that errors are processed as expected.
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
    /// Ensures that when the model state is invalid, the <see cref="ObstacleController.DataformStep2()"/> action
    /// returns the expected view for Dataform Step 2, validating its error-handling behavior.
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
    /// Validates the behavior of the <see cref="ObstacleController.DataformStep3()"/> action method
    /// by ensuring that it returns the DataformStep3 view when the ModelState is invalid.
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