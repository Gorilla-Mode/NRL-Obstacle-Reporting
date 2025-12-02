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
    public void PilotDrafts_ReturnsPilotDraftsView()
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
    /// Validates that the <see cref="DraftController.PilotDrafts"/> method throws an <see cref="InvalidOperationException"/>
    /// when the user ID cannot be retrieved.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current user ID is null or cannot be determined.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Thrown when exception is asserted before aggregate is unwrapped. Not strictly a necessary test
    /// </exception>
    [Fact]
    public void PilotDrafts_InvalidUserIdThrowsInvalidOperationException()
    {
        //arrange
        var controller = CreateDraftController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = Substitute.For<HttpContext>()
        };
        
        // Act
        //DO NOT MESS. Defers call to assertion to avoid internal throw outside assert context 
        var unwrappedResult = () => controller.PilotDrafts().GetAwaiter().GetResult(); //Test the actual exception in controller
        var wrappedResult = () => controller.PilotDrafts().Result;
        
        //assert
        Assert.Throws<InvalidOperationException>(unwrappedResult);
        Assert.Throws<AggregateException>(wrappedResult);
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
    public void PilotDrafts_SetsViewDataWithDrafts()
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
            new ObstacleCompleteModel { ObstacleId = "1", UserId = userId },
            new ObstacleCompleteModel { ObstacleId = "2", UserId = userId }
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
    public void SubmitDraft_InvalidModelStateReturnsSubmitDraftView()
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
    
    
}
    