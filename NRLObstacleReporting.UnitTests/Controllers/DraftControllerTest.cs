using AutoMapper;
using JetBrains.Annotations;
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

    private DraftController CreateDraftController()
    {
        _mapper = Substitute.For<IMapper>();
        _draftRepository = Substitute.For<IDraftRepository>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>();
        var controller = new DraftController(_mapper, _draftRepository, _signInManager);
        return controller;
    }
    
    [Fact]
    public void PilotDraftsReturnsPilotDraftsView()
    {
        //arrange
        var controller = CreateDraftController();

        //act
        var result = controller.PilotDrafts();
        var viewResult = result.Result as ViewResult;
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    //checks that code takes appropriate path on invalid model state
    [Fact]
    public void SubmitDraftInvalidModelStateReturnsSubmitDraftView()
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