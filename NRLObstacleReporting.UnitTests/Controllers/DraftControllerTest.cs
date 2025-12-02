using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
public class DraftControllerTest
{
    private IMapper _mapper;
    private IDraftRepository _draftRepository;
    private SignInManager<IdentityUser> _signInManager;

    /// <summary>
    /// Creates and initializes an instance of <see cref="DraftController"/>, with the proper dependencies.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="DraftController"/>.
    /// </returns>
    private DraftController CreateDraftController()
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
        
        _mapper = Substitute.For<IMapper>();
        _draftRepository = Substitute.For<IDraftRepository>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>(
            userManager, httpContextAccessor, Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(),
            null,
            null,
            null, 
            null);
        
        var controller = new DraftController(_mapper, _draftRepository, _signInManager);

        return controller;
    }

    /// <summary>
    /// Verifies that the <see cref="DraftController.PilotDrafts"/> method
    /// returns a view result with the appropriate output when invoked.
    /// </summary>
    [Fact]
    public void PilotDraftsGET_ReturnsPilotDraftsView()
    {
        //arrange
        var controller = CreateDraftController();

        //act
        var result = controller.PilotDrafts();
        var viewResult = result.Result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    /// <summary>
    /// Tests that methods in <see cref="DraftController"/> throw appropriate exceptions when accessed by an invalid or unauthorized user.
    /// </summary>
    /// <remarks>
    /// This test verifies that each method in <see cref="DraftController"/> correctly throws an <see cref="InvalidOperationException"/>
    /// And an <see cref="AggregateException"/> which is not strictly necessary, but it confirms it's a task.
    /// </remarks>
    [Fact]
    public void DraftController_MethodsThrowOnInvalidUser()
    {
        //arrange
        var controller = CreateDraftController();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        
        // Act
        //DO NOT MESS. Defers call to assertion to avoid internal throw outside assert context
        var pilotDraftUnwrappedResult = () => controller.PilotDrafts().GetAwaiter().GetResult(); //Test the actual exception in controller
        var pilotDraftWrappedResult = () => controller.PilotDrafts().Result;
        
        var editDraftUnwrappedResult = () => controller.EditDraft("1").GetAwaiter().GetResult(); //Test the actual exception in controller
        var editDraftWrappedResult = () => controller.EditDraft("1").Result;
        
        var saveEditedDraftUnwrappedResult = () => controller.SaveEditedDraft(Substitute.For<ObstacleCompleteModel>()).GetAwaiter().GetResult(); //Test the actual exception in controller
        var saveEditedDraftWrappedResult = () => controller.SaveEditedDraft(Substitute.For<ObstacleCompleteModel>()).Result;
        
        var submitDraftUnwrappedResult = () => controller.SubmitDraft(Substitute.For<ObstacleCompleteModel>()).GetAwaiter().GetResult(); //Test the actual exception in controller
        var submitDraftWrappedResult = () => controller.SubmitDraft(Substitute.For<ObstacleCompleteModel>()).Result;
        
        //assert
        Assert.Throws<InvalidOperationException>(pilotDraftUnwrappedResult);
        Assert.Throws<AggregateException>(pilotDraftWrappedResult);
        
        Assert.Throws<InvalidOperationException>(editDraftUnwrappedResult);
        Assert.Throws<AggregateException>(editDraftWrappedResult);
        
        Assert.Throws<InvalidOperationException>(saveEditedDraftUnwrappedResult);
        Assert.Throws<AggregateException>(saveEditedDraftWrappedResult);
        
        Assert.Throws<InvalidOperationException>(submitDraftUnwrappedResult);
        Assert.Throws<AggregateException>(submitDraftWrappedResult);
    }

    /// <summary>
    /// Ensures that the "Drafts" key of <see cref="ViewDataDictionary"/> is populated with the drafts
    /// associated with the currently signed-in user, ASSUMING appropriate mapping from DTOs to the view model, and
    /// that the dapper repos behave.
    /// </summary>
    /// <remarks>
    /// Asserts that viewdata isn't empty, containins "Drafts" key a and that the contents match expected result. Aslo
    /// assert that the method returns proper view
    /// </remarks>
    [Fact]
    public void PilotDraftsGET_SetsViewDataWithDrafts()
    {
        // Arrange
        var controller = CreateDraftController();
        const string userId = "test-user-id";
        const string viewDataKey = "Drafts";
        
        var draftDto = new List<ObstacleDto>
        {
            new ObstacleDto
            {
                ObstacleId = "1",
                UserId = userId,
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft
            },
            new ObstacleDto
            {
                ObstacleId = "2",
                UserId = userId,
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft
            }
        };
        _draftRepository.GetAllDraftsAsync(userId).Returns(draftDto);
        
        var draftViewModel = new List<ObstacleCompleteModel>
        {
            new ObstacleCompleteModel
                { ObstacleId = "1",
                    UserId = userId 
                },
            new ObstacleCompleteModel 
                { 
                    ObstacleId = "2",
                    UserId = userId 
                }
        };
        _signInManager.UserManager.GetUserId(Arg.Any<ClaimsPrincipal>()).Returns(userId);
        _mapper.Map<IEnumerable<ObstacleCompleteModel>>(draftDto).Returns(draftViewModel);

        // Act
        var result =  controller.PilotDrafts().Result;
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(viewResult!.ViewName);
        Assert.True(viewResult.ViewData.ContainsKey(viewDataKey));
        
        var draftsInViewData = viewResult.ViewData[viewDataKey] as IEnumerable<ObstacleCompleteModel>;
        Assert.Equal(draftViewModel, draftsInViewData);
    }
        
    /// <summary>
    /// Tests if the <see cref="DraftController.SubmitDraft(ObstacleCompleteModel)"/> method
    /// returns the "EditDraft" view when the provided model state is invalid.
    /// </summary>
    /// <remarks>
    /// Adds a model state error to simulate an invalid model, invokes the method,
    /// and asserts that the result is the expected view.
    /// </remarks>
    [Fact]
    public void SubmitDraftPOST_InvalidModelStateReturnsSubmitDraftView()
    {
        //arrange
        var controller = CreateDraftController();
        var model = Substitute.For<ObstacleCompleteModel>(); //Creates substitute model for method
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");

        //act
        var result = controller.SubmitDraft(model);
        var viewResult = result.Result as ViewResult;

        //assert
        Assert.Equal("EditDraft", viewResult!.ViewName);
    }
    
    
    /// <summary>
    /// Tests that the <see cref="DraftController.SubmitDraft(ObstacleCompleteModel)"/> method
    /// redirects to the PilotDrafts action when the provided model state is valid.
    /// </summary>
    /// <remarks>
    /// Provides a valid model state, invokes the method, and checks that the result is a redirect to PilotDrafts.
    /// </remarks>
    [Fact]
    public void SubmitDraftPOST_ValidModelStateRedirectsToPilotDrafts()
    {
        //arrange
        var controller = CreateDraftController();
        
        const string expectedAction = "PilotDrafts";
        var model = Substitute.For<ObstacleCompleteModel>(); // Creates substitute model for method

        //act
        var result = controller.SubmitDraft(model);
        var redirectResult = result.Result as RedirectToActionResult;

        //assert
        Assert.NotNull(result);
        Assert.NotNull(redirectResult);
        Assert.Equal(expectedAction, redirectResult!.ActionName);
    }

    /// <summary>
    /// Tests the behavior of the EditDraft method in the <see cref="DraftController"/> when provided with a valid draft ID
    /// for an authenticated user.
    /// Ensures that the method returns the EditDraft view with the expected model.
    /// </summary>
    [Fact]
    public void EditDraftGET_ReturnsEditDraftViewWithModel()
    {
        // Arrange
        var controller = CreateDraftController();
        
        const string draftId = "test-draft-id";
        const string userId = "test-user-id";

        var draftDto = new ObstacleDto
        {
            ObstacleId = draftId,
            UserId = userId,
            Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft
        };

        var draftViewModel = new ObstacleCompleteModel
        {
            ObstacleId = draftId,
            UserId = userId,
            Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft
        };
        
        _signInManager.UserManager.GetUserId(Arg.Any<ClaimsPrincipal>()).Returns(userId);
        _draftRepository.GetDraftByIdAsync(draftId, userId).Returns(draftDto);
        _mapper.Map<ObstacleCompleteModel>(draftDto).Returns(draftViewModel);

        // Act
        var result = controller.EditDraft(draftId).Result;
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(viewResult);
        Assert.Null(viewResult!.ViewName);
        Assert.Equal(draftViewModel, viewResult.Model);
    }
    
    /// <summary>
    /// Tests that the <see cref="DraftController.SaveEditedDraft(ObstacleCompleteModel)"/> method
    /// redirects to the PilotDrafts action when the provided model state is valid.
    /// </summary>
    [Fact]
    public void SaveEditedDraftPOST_RedirectsToPilotDrafts()
    {
        // Arrange
        var controller = CreateDraftController();
        
        const string expectedAction = "PilotDrafts";
        var model = Substitute.For<ObstacleCompleteModel>(); 

        // Act
        var result = controller.SaveEditedDraft(model);
        var redirectResult = result.Result as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(redirectResult);
        Assert.Equal(expectedAction, redirectResult!.ActionName);
    }

    /// <summary>
    /// Tests that the SaveEditedDraft action method of <see cref="DraftController"/>
    /// returns the EditDraft view when the model state is invalid. Confimring that a user is able so save an obstacle
    /// as a draft even if model state is invalid
    /// </summary>
    [Fact]
    public void SaveEditedDraftPOST_SavingDraftAllowsInvalidModelState()
    {
        // Arrange
        var controller = CreateDraftController();

        const string expectedAction = "PilotDrafts";
        var model = Substitute.For<ObstacleCompleteModel>();
        
        controller.ModelState.AddModelError("Error", "Invalid model state");

        // Act
        var result = controller.SaveEditedDraft(model).Result;
        var redirectResult = result as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(redirectResult);
        Assert.Equal(expectedAction, redirectResult!.ActionName);
    }
}