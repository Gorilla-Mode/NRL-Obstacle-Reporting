using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NRLObstacleReporting.Controllers;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(HomeController))]
public class HomeControllerTest
{
    private UserManager<IdentityUser> _userManager;
    private ILogger<HomeController> _logger;

    /// <summary>
    /// Creates and initializes an instance of <see cref="HomeController"/>, with the proper dependencies.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="HomeController"/>.
    /// </returns>
    private HomeController CreateHomeController()
    {
        _logger =  Substitute.For<ILogger<HomeController>>();
        
        _userManager = Substitute.For<UserManager<IdentityUser>>(
            Substitute.For<IUserStore<IdentityUser>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
        
        return new HomeController(_logger, _userManager);
    }


    /// <summary>
    /// Adds a simulated user to the specified controller, and optionally assigns a specific role for testing purposes.
    /// </summary>
    /// <param name="controller">Controller object instance to assign the mocked user context.</param>
    /// <param name="userId">The id of the simulated user</param>
    /// <param name="role">The role assigned to the user. Optional, defaults to Null</param>
    private void CreateUserSubstitute(HomeController controller, string userId, string role = null)
    {
        var user = new IdentityUser { Id = userId };

        var claimIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) });
        var claimPrincipal = new ClaimsPrincipal(claimIdentity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimPrincipal }
        };

        _userManager.FindByIdAsync(userId).Returns(user);

        // Allow user without role, for testing
        if (!string.IsNullOrEmpty(role))
        {
            _userManager.IsInRoleAsync(user, role).Returns(true);
        }
    }

    /// <summary>
    /// Verifies that the <see cref="HomeController.Index"/> action redirects to the "PilotIndex" action
    /// in the "Pilot" controller when the logged-in user has the "Pilot" role.
    /// </summary>
    [Fact]
    public void IndexRedirectsToPilotRole()
    {
        // Arrange
        var controller = CreateHomeController();
        CreateUserSubstitute(controller, "dummyUserId", "Pilot");

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("PilotIndex", redirectResult.ActionName);
        Assert.Equal("Pilot", redirectResult.ControllerName);
    }

    /// <summary>
    /// Verifies that the <see cref="HomeController.Index"/> action redirects to the appropriate action and controller
    /// when a user is assigned the "Registrar" role.
    /// </summary>
    /// <remarks>
    /// This test ensures that users with the "Registrar" role are redirected to the "RegistrarViewReports" action
    /// of the "Registrar" controller when accessing the Index action.
    /// </remarks>
    [Fact]
    public void IndexRedirectsToRegistrarRole()
    {
        // Arrange
        var controller = CreateHomeController();
        CreateUserSubstitute(controller, "dummyUserId", "Registrar");

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RegistrarViewReports", redirectResult.ActionName);
        Assert.Equal("Registrar", redirectResult.ControllerName);
    }

    /// <summary>
    /// Tests whether the Index action of the <see cref="HomeController"/>
    /// redirects to the appropriate action and controller when the user has the "Administrator" role.
    /// </summary>
    /// <remarks>
    /// This method ensures that users with the "Administrator" role are correctly redirected
    /// to the "AdminIndex" action in the "Admin" controller.
    /// </remarks>
    [Fact]
    public void IndexRedirectsToAdministratorRole()
    {
        // Arrange
        var controller = CreateHomeController();
        CreateUserSubstitute(controller, "dummyUserId", "Administrator");

        // Act
        var result = controller.Index().Result; 

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AdminIndex", redirectResult.ActionName);
        Assert.Equal("Admin", redirectResult.ControllerName);
    }

    /// <summary>
    /// Verifies that the <see cref="HomeController.Index"/> method returns a <see cref="ViewResult"/>
    /// when the current user is not assigned any role.
    /// </summary>
    /// <remarks>
    /// This test simulates a user without any specific roles attached, and verifies that the
    /// result of the <see cref="HomeController.Index"/> method is a view result, indicating that
    /// the user can access the default view in the absence of a role-based redirection path.
    /// </remarks>
    [Fact]
    public void IndexReturnsViewForNoRole()
    {
        // Arrange
        var controller = CreateHomeController();
        CreateUserSubstitute(controller, "dummyUserId");

        // Ensure no roles are set to true.
        _userManager.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(false);

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    /// <summary>
    /// Confirms that the <see cref="HomeController.Privacy"/> action method
    /// returns a view result with no specific view name.
    /// </summary>
    [Fact]
    public void PrivacyReturnsPrivacyView()
    {
        //arrange
        var controller = CreateHomeController();
        
        //act 
        var result = controller.Privacy();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null(viewResult!.ViewName);
    }
}