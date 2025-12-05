using System;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Models;
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

    /// <summary>
    /// Ensures that the <see cref="RegistrarController.RegistrarViewReports"/> method
    /// returns the correct view for displaying reports submitted to the registrar.
    /// </summary>
    [Fact]
    public void RegistrarViewReportsRegistrarGET_ReturnsViewReportsRegistrarView()
    {
        //arrange
        var controller = CreateRegistrarController();

        //act
        var result = controller.RegistrarViewReports();
        var viewResult = result.Result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Ensures that the <see cref="RegistrarController.RegistrarAcceptReport(string)"/> method
    /// returns the correct view for displaying a report that has been accepted by the registrar.
    /// </summary>
    [Fact]
    public void RegistrarAcceptReportGET_ReturnsViewReportsRegistrarView()
    {
        // Arrange
        var controller = CreateRegistrarController();
        const string reportedObstacleId = "test-id"; // Assumes obstacle is valid and exists

        // Act
        var result =  controller.RegistrarAcceptReport(reportedObstacleId).Result; //async to synchronous

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result); 
        Assert.Equal(null, viewResult.ViewName);    
    }

    /// <summary>
    /// Tests whether the <see cref="RegistrarController.UpdateReportStatus(ObstacleCompleteModel)"/> method
    /// redirects to the "RegistrarAcceptReport" action with a mock model.
    /// </summary>
    [Fact]
    public void UpdateReportStatusPOST_RedirectsToRegistrarAcceptReport_WithMockModel()
    {
        // Arrange
        var controller = CreateRegistrarController();
        var mockModel = Substitute.For<ObstacleCompleteModel>();

        // Act
        var result = controller.UpdateReportStatus(mockModel).Result; // async to synchronous

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RegistrarAcceptReport", redirectResult.ActionName);
        Assert.Equal(mockModel.ObstacleId, redirectResult.RouteValues!["id"]);
    }

    /// <summary>
    /// Ensures that the <see cref="RegistrarController.RegistrarFilterReports"/> method redirects
    /// to the correct view, validating the resulting view name is "RegistrarViewReports".
    /// </summary>
    [Fact]
    public void RegistrarFilterReportsGET_RedirectsToCorrectView()
    {
        // Arrange
        var controller = CreateRegistrarController(); 

        // Act
        var result = controller.RegistrarFilterReports(
            new [] { ObstacleCompleteModel.ObstacleStatus.Approved },
            new [] { ObstacleCompleteModel.ObstacleTypes.Bridge },
            new [] { ObstacleCompleteModel.Illumination.NotIlluminated },
            new [] { ObstacleCompleteModel.ObstacleMarking.Marked },
            DateOnly.MinValue, 
            DateOnly.MaxValue
            ).Result; // async to synchronous

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result); 
        Assert.Equal("RegistrarViewReports", viewResult.ViewName); 
    }
    
    /// <summary>
    /// Verifies that <see cref="RegistrarController.RegistrarFilterReports"/> returns a <see cref="BadRequestObjectResult"/>
    /// when the model state is invalid and ensures that the resulting error messages match the expected values.
    /// </summary>
    [Fact]
    public void RegistrarFilterReportsGET_ThrowsBadHttpRequestExceptionOnInvalidModelState()
    {
        // Arrange
        var controller = CreateRegistrarController();
        const string errorKey = "TestError";
        const string errorMessage = "Invalid model state";

        // Simulate invalid model state
        controller.ModelState.AddModelError(errorKey, errorMessage);

        // Act & Assert
        var exception = Assert.ThrowsAsync<BadHttpRequestException>(() =>
            controller.RegistrarFilterReports(
                new[] { ObstacleCompleteModel.ObstacleStatus.Approved },
                new[] { ObstacleCompleteModel.ObstacleTypes.Bridge },
                new[] { ObstacleCompleteModel.Illumination.NotIlluminated },
                new[] { ObstacleCompleteModel.ObstacleMarking.Marked },
                DateOnly.MinValue,
                DateOnly.MaxValue
            )
        ).Result;

        // Validate exception message
        Assert.Contains($"Field '{errorKey}': {errorMessage}", exception.Message);
    }
}

