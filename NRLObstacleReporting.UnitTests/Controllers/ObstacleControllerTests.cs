using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Database;
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
    public void DataformStep1GET_ReturnsDataformStep1View()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Ensures that <see cref="ObstacleController.DataformStep2()"/> action method in the <see cref="ObstacleController"/>
    /// returns the appropriate view, validating its functionality.
    /// </summary>
    [Fact]
    public void DataformStep2GET_ReturnsDataformStep2View()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Ensures that <see cref="ObstacleController.DataformStep3()"/> action method in the <see cref="ObstacleController"/>
    /// returns the appropriate view, validating its functionality.
    /// </summary>
    [Fact]
    public void DataformStep3GET_ReturnsOverviewView()
    {
        //arrange
        var controller = CreateObstacleController();

        //act
        var result = controller.DataformStep3();
        var viewResult = result as ViewResult;

        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Validates that when the model state is invalid in the <see cref="ObstacleController.DataformStep1()"/> action method,
    /// the appropriate view is returned, ensuring that errors are processed as expected.
    /// </summary>
    [Fact]
    public void DataFormStep1POST_InvalidModelStateReturnsDataformStep1View()  
    {
        //arrange
        var controller = CreateObstacleController();
        
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Validates the provided <see cref="ObstacleStep1Model"/> with a valid model state
    /// and redirects to the Dataform Step 2 view when the model is valid.
    /// </summary>
    /// <remarks>
    /// This method verifies that the model submitted in the POST request has no validation errors
    /// and proceeds to redirect the user to the next step in the data form sequence.
    /// </remarks>
    [Fact]
    public void DataformStep1_ValidModelRedirectsToDataformStep2GET()
    {
        // Arrange
        var controller = CreateObstacleController();
        
        const string expectedAction = nameof(controller.DataformStep2);
        
        var httpContext = Substitute.For<HttpContext>();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var tempData = new TempDataDictionary(httpContext, tempDataProvider);
        controller.TempData = tempData;

        var obstacleModel = new ObstacleStep1Model
        {
            SaveDraft = false,
            Type = ObstacleCompleteModel.ObstacleTypes.Bridge,
            HeightMeter = 14
        };

        // Act
        var result = controller.DataformStep1(obstacleModel).Result;
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(redirectResult);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(expectedAction, redirectResult!.ActionName);
    }

    /// <summary>
    /// Ensures that when the model state is invalid, the <see cref="ObstacleController.DataformStep2()"/> action
    /// returns the expected view for Dataform Step 2, validating its error-handling behavior.
    /// </summary>
    [Fact]
    public void DataFormStep2POST_InvalidModelStateReturnsDataformStep2View()
    {
        //arrange
        var controller = CreateObstacleController();
        
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Tests that when a valid <see cref="ObstacleStep2Model"/> is provided to the <see cref="ObstacleController.DataformStep2()"/>
    /// action, the method redirects to the <see cref="ObstacleController.DataformStep3()"/> action.
    /// </summary>
    /// <remarks>
    /// Ensures that the <see cref="ObstacleController"/> processes valid obstacle data from
    /// step 2 of the data form correctly, leading to a redirection to the next step of the workflow.
    /// </remarks>
    [Fact]
    public void DataformStep2_ValidModelRedirectsToDataformStep3GET()
    {
        // Arrange
        var controller = CreateObstacleController();
        
        const string expectedAction = nameof(controller.DataformStep3);
        
        var httpContext = Substitute.For<HttpContext>();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var tempData = new TempDataDictionary(httpContext, tempDataProvider);
        controller.TempData = tempData;

        var obstacleModel = new ObstacleStep2Model
        {
            SaveDraft = false,
            GeometryGeoJson = "Suspicious geojson"
        };

        // Act
        var result = controller.DataformStep2(obstacleModel).Result;
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(redirectResult);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(expectedAction, redirectResult!.ActionName);
    }

    /// <summary>
    /// Tests that the <see cref="ObstacleController.DataformStep3()"/> method returns an `Overview` view
    /// with a model of type <see cref="ObstacleCompleteModel"/>, when provided with a valid instance of <see cref="ObstacleStep3Model"/>.
    /// </summary>
    [Fact]
    public void DataformStep3_ValidModel_ReturnsOverviewViewWithModel()
    {
        // Arrange
        var controller = CreateObstacleController();
        
        const string testUser = "test-user";
        const string expectedViewName = "Overview";
        var step3Model = new ObstacleStep3Model
        {
            ObstacleId = "1",
            Description = "LALAA",
            Illuminated = ObstacleCompleteModel.Illumination.Illuminated,
            Status = ObstacleCompleteModel.ObstacleStatus.Pending,
            Marking = ObstacleCompleteModel.ObstacleMarking.Unknown,
            UserId = testUser
        };
        
        var obstacleDto = new ObstacleDto
        {
            ObstacleId = "1",
            Description = "LALAA",
            Illuminated = (int)ObstacleCompleteModel.Illumination.Illuminated,
            Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
            Marking = (int)ObstacleCompleteModel.ObstacleMarking.Unknown,
            UserId = testUser,
            GeometryGeoJson = "suspicious geojson",
            HeightMeter = 12
        };

        var expectedModel = new ObstacleCompleteModel
        {
            ObstacleId = "1",
            Description = "LALAA",
            Illuminated = ObstacleCompleteModel.Illumination.Illuminated,
            Status = ObstacleCompleteModel.ObstacleStatus.Pending,
            Marking = ObstacleCompleteModel.ObstacleMarking.Unknown,
            UserId = testUser,
            GeometryGeoJson = "suspicious geojson",
            HeightMeter = 12
            
        };
        
        _obstacleRepository.GetObstacleByIdAsync(step3Model.ObstacleId).Returns(obstacleDto);
        _mapper.Map<ObstacleDto>(step3Model).Returns(obstacleDto);
        _mapper.Map<ObstacleCompleteModel>(obstacleDto).Returns(expectedModel);

        // Act
        var result = controller.DataformStep3(step3Model).Result;
        var viewResult = result as ViewResult;

        // Assert
         Assert.IsType<ViewResult>(result);
        Assert.Equal(expectedViewName, viewResult!.ViewName); 
        Assert.NotNull(viewResult.Model); 
        Assert.IsType<ObstacleCompleteModel>(viewResult.Model); 
        Assert.Equal(expectedModel, viewResult.Model); 
    }
}