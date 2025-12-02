using System;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
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
    