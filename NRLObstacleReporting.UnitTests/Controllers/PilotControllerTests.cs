using AutoMapper;
using JetBrains.Annotations;
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

    private PilotController CreatePilotController()
    {
        _obstacleRepository = Substitute.For<IObstacleRepository>();
        _mapper = Substitute.For<IMapper>();
        var controller = new PilotController(_obstacleRepository, _mapper);
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
    
    // [Fact]
    // public void PilotDraftsReturnsPilotDraftsView()
    // {
    //     //arrange
    //     var controller = new PilotController();
    //     
    //     //act
    //     var result = controller.PilotDrafts();
    //     var viewResult = result as ViewResult;
    //     //assert
    //     Assert.Equal(null, viewResult!.ViewName);
    // }
    
    
}