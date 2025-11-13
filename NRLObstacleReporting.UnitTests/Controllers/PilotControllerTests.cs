using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
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

    private PilotController CreatePilotController()
    {
        _obstacleRepository = Substitute.For<IObstacleRepository>();
        _mapper = Substitute.For<IMapper>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>();
        var controller = new PilotController(_obstacleRepository, _mapper, _signInManager);
        return controller;
    }
    
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
    
    
    
    
}