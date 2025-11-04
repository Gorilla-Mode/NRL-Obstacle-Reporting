using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Repositories;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class RegistrarControllerTests
{
    private IMapper _mapper;
    private IObstacleRepository _obstacleRepository;
    
    private RegistrarController CreateRegistrarController()
    {
        _mapper = Substitute.For<IMapper>();
        _obstacleRepository = Substitute.For<IObstacleRepository>();
        var controller = new RegistrarController(_mapper, _obstacleRepository);
        
        return controller;
    }

    [Fact]
    public void RegistrarIndexReturnsRegistrarIndexView()
    {
        //arrange
        var controller = CreateRegistrarController();

        //act
        var result = controller.RegistrarIndex();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }
    

    [Fact]
    public void RegistrarViewReportsRegistrarReturnsViewReportsRegistrarView()
    {
        //arrange
        var controller = CreateRegistrarController();

        //act
        var result = controller.RegistrarViewReports();
        var viewResult = result.Result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }
}