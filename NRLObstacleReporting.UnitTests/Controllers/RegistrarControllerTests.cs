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
public class RegistrarControllerTests
{
    private IMapper _mapper;
    private IRegistrarRepository _registrarRepository;
    
    /// <summary>
    /// Creates and initializes an instance of <see cref="RegistrarController"/>, with the proper dependencies.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="RegistrarController"/>.
    /// </returns>
    private RegistrarController CreateRegistrarController()
    {
        _mapper = Substitute.For<IMapper>();
        _registrarRepository = Substitute.For<IRegistrarRepository>();
        
        var controller = new RegistrarController(_mapper, _registrarRepository);
        
        return controller;
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