using System.Security.Claims;
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
    
    //TODO ADD DOCSTRING
    [Fact]
    public void DataformStep1POST_SaveDraftValidModelReturnsOverviewView()
    {
        // Arrange
        var controller = CreateObstacleController();
        
        var obstacleModel = new ObstacleStep1Model
        {
            SaveDraft = true,
            Type = ObstacleCompleteModel.ObstacleTypes.Bridge,
            HeightMeter = 14
        };
        var obstacleDto = new ObstacleDto
        {
            ObstacleId = "some-id",
            UserId = "user-id",
            Type = (int)ObstacleCompleteModel.ObstacleTypes.Bridge,
            HeightMeter = 14
        };
        var expectedModel = new ObstacleCompleteModel
        {
            ObstacleId = "some-id",
            UserId = "user-id",
            Type = ObstacleCompleteModel.ObstacleTypes.Bridge,
            HeightMeter = 14
        };

        _obstacleRepository.InsertStep1Async(Arg.Any<ObstacleDto>()).Returns(Task.CompletedTask);
        _obstacleRepository.GetObstacleByIdAsync(Arg.Any<string>()).Returns(obstacleDto);
        _mapper.Map<ObstacleCompleteModel>(obstacleDto).Returns(expectedModel);

        // Act
        var result = controller.DataformStep1(obstacleModel).Result;
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(viewResult);
        Assert.Equal("Overview", viewResult!.ViewName);
        Assert.NotNull(viewResult.Model);
        Assert.IsType<ObstacleCompleteModel>(viewResult.Model);
        Assert.Equal(expectedModel, viewResult.Model);
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
    
    //TODO ADD DOCSTRING
    [Fact]
    public void DataformStep2POST_SaveDraftValidModelReturnsOverviewView()
    {
        //Arrange
        var controller = CreateObstacleController();

        const string testUser = "test-user-id";
        const string expectedViewName = "Overview";
        const string obstacleId = "22";
        const string key = "id";

        var httpContext = Substitute.For<HttpContext>();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, testUser) };

        //mock up temp-data to access
        controller.TempData = new TempDataDictionary(httpContext, tempDataProvider)
        {
            [key] = obstacleId
        };

        //simulate a logged-in user
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, testUser) }))
            }
        };

        var obstacleModel = new ObstacleStep2Model
        {
            SaveDraft = true,
            GeometryGeoJson = "Bing bong land",
        };
        var obstacleDto = new ObstacleDto
        {
            ObstacleId = obstacleId,
            UserId = testUser,
            GeometryGeoJson = "Bing bong land",
        };
        var expectedModel = new ObstacleCompleteModel
        {
            ObstacleId = obstacleId,
            UserId = testUser,
            GeometryGeoJson = "Bing bong land",
        };
        
        _obstacleRepository.InsertStep2Async(Arg.Any<ObstacleDto>()).Returns(Task.CompletedTask);
        _obstacleRepository.GetObstacleByIdAsync(Arg.Any<string>()).Returns(obstacleDto);
        _mapper.Map<ObstacleCompleteModel>(obstacleDto).Returns(expectedModel);
        
        //Act
        var result = controller.DataformStep2(obstacleModel).Result;
        var viewResult = result as ViewResult;
        
        //Assert
        Assert.NotNull(viewResult);
        Assert.Equal("Overview", viewResult!.ViewName);
        Assert.NotNull(viewResult.Model);
        Assert.IsType<ObstacleCompleteModel>(viewResult.Model);
        Assert.Equal(expectedModel, viewResult.Model);
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
    /// Tests that the DataformStep3 POST action of the controller correctly processes a valid model.
    /// Verifies that the action returns the "Overview" view with a model of type <see cref="ObstacleCompleteModel"/>
    /// populated with the expected data.
    /// </summary>
    [Fact]
    public void DataformStep3POST_ValidModel_ReturnsOverviewViewWithModel()
    {
        // Arrange
        var controller = CreateObstacleController();

        const string testUser = "test-user-id"; 
        const string expectedViewName = "Overview";
        const string obstacleId = "22";
        const string key = "id";
        
        var httpContext = Substitute.For<HttpContext>();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, testUser) };
        
        //mock up temp-data to access
        controller.TempData = new TempDataDictionary(httpContext, tempDataProvider)
        {
            [key] = obstacleId
        };
        
        //simulate a logged-in user
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            }
        };
        
        var step3Model = new ObstacleStep3Model
        {
            Description = "LALAA",
            Illuminated = ObstacleCompleteModel.Illumination.Illuminated,
            Status = ObstacleCompleteModel.ObstacleStatus.Pending,
            Marking = ObstacleCompleteModel.ObstacleMarking.Unknown
        };

        var obstacleDto = new ObstacleDto
        {
            ObstacleId = obstacleId,
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
            ObstacleId = obstacleId,
            Description = "LALAA",
            Illuminated = ObstacleCompleteModel.Illumination.Illuminated,
            Status = ObstacleCompleteModel.ObstacleStatus.Pending,
            Marking = ObstacleCompleteModel.ObstacleMarking.Unknown,
            UserId = testUser,
            GeometryGeoJson = "suspicious geojson",
            HeightMeter = 12
        };
        
        _obstacleRepository.InsertStep3Async(Arg.Any<ObstacleDto>()).Returns(Task.CompletedTask); //1. user insert model
        _obstacleRepository.GetObstacleByIdAsync(controller.TempData[key].ToString()).Returns(obstacleDto); //2. full obstacle return from db
        _mapper.Map<ObstacleCompleteModel>(obstacleDto).Returns(expectedModel); //3. full obstacle mapped to view model
        
        // Act
        var result = controller.DataformStep3(step3Model).Result;
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(viewResult);
        Assert.Equal(expectedViewName, viewResult!.ViewName);
        Assert.NotNull(viewResult.Model);
        Assert.IsType<ObstacleCompleteModel>(viewResult.Model);
        Assert.Equal(expectedModel, viewResult.Model);
    }
}