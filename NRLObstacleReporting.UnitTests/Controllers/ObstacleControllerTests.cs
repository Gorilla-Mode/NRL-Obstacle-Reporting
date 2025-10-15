using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Models;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class ObstacleControllerTests
{
    /// <summary>
    /// Method Creates objectcontroller instance
    /// </summary>
    /// <returns></returns>
    private static ObstacleController InstanitateObstacleController()
    {
        var controller = new ObstacleController();
        return controller;
    }
    [Fact]
    public void DataformStep1ReturnsDataformStep1View()
    {
        //arrange
        var controller = InstanitateObstacleController();

        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    [Fact]
    public void DataformStep2ReturnsDataformStep2View()
    {
        //arrange
        var controller = InstanitateObstacleController();

        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    [Fact]
    public void DataformStep3ReturnsOverviewView()
    {
        //arrange
        var controller = InstanitateObstacleController();

        //act
        var result = controller.DataformStep3();
        var viewResult = result as ViewResult;

        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }

    //checks that code takes appropriate path on invalid model state
    [Fact]
    public void DataFormStep1InvalidModelStateReturnsDataformStep1View()  
    {
        //arrange
        var controller = InstanitateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep1();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    //checks that code takes appropriate path on invalid model state
    [Fact]
    public void DataFormStep2InvalidModelStateReturnsDataformStep2View()
    {
        //arrange
        var controller = InstanitateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep2();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    //checks that code takes appropriate path on invalid model state
    [Fact]
    public void DataFormStep3InvalidModelStateReturnsDataformStep3View()
    {
        //arrange
        var controller = InstanitateObstacleController();
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.DataformStep3();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal(null, viewResult!.ViewName);
    }
    
    //checks that code takes appropriate path on invalid model state
    [Fact]
    public void SubmitDraftInvalidModelStateReturnsSubmitDraftView()
    {
        //arrange
        var controller = InstanitateObstacleController();
        var model = Substitute.For<ObstacleCompleteModel>(); //Creates substitute model for method
        //adds error to model state
        controller.ModelState.AddModelError("ObstacleHeightMeter", "Obstacle height meter is required.");
        
        //act
        var result = controller.SubmitDraft(model);
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Equal("EditDraft", viewResult!.ViewName);
    }
    
}