using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Models;
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
    /// Method Creates objectcontroller instance
    /// </summary>
    /// <returns></returns>
    private ObstacleController CreateObstacleController()
    {
        _obstacleRepository = Substitute.For<IObstacleRepository>();
        _mapper = Substitute.For<IMapper>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>();
        var controller = new ObstacleController(_obstacleRepository, _mapper, _signInManager);
        return controller;
    }
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

    //checks that code takes appropriate path on invalid model state
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
    
    //checks that code takes appropriate path on invalid model state
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
    
    //checks that code takes appropriate path on invalid model state
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