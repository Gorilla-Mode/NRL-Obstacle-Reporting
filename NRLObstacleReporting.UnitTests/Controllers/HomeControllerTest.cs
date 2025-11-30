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
    /// Method Creates model with a substitute for logger interface, and user manager
    /// </summary>
    /// <returns>Controller with proper subsituted dependencies</returns>
    private HomeController HomeControllerLogger()
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
    /// Verifies that the Index action of HomeController redirects to "PilotIndex" view within the "Pilot" controller
    /// when a user with the "Pilot" role is authenticated.
    /// </summary>
    [Fact]
    public void IndexRedirectsToPilotRole()
    {
        // Arrange
        var controller = HomeControllerLogger();
        CreateUserSubstitute(controller, "dummyUserId", "Pilot");

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("PilotIndex", redirectResult.ActionName);
        Assert.Equal("Pilot", redirectResult.ControllerName);
    }

    /// <summary>
    /// Verifies that the Index action of HomeController redirects to "RegistrarViewReports" view within
    /// the "Registrar" controller when a user with the "Registrar" role is authenticated.
    /// </summary>
    [Fact]
    public void IndexRedirectsToRegistrarRole()
    {
        // Arrange
        var controller = HomeControllerLogger();
        CreateUserSubstitute(controller, "dummyUserId", "Registrar");

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RegistrarViewReports", redirectResult.ActionName);
        Assert.Equal("Registrar", redirectResult.ControllerName);
    }

    /// <summary>
    /// Verifies that the Index action of HomeController redirects to "AdminIndex" view within the "Admin" controller
    /// when a user with the "Administrator" role is authenticated.
    /// </summary>
    [Fact]
    public void IndexRedirectsToAdministratorRole()
    {
        // Arrange
        var controller = HomeControllerLogger();
        CreateUserSubstitute(controller, "dummyUserId", "Administrator");

        // Act
        var result = controller.Index().Result; 

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AdminIndex", redirectResult.ActionName);
        Assert.Equal("Admin", redirectResult.ControllerName);
    }

    /// <summary>
    /// Validates that the Index action of HomeController returns Index when the user has no assigned role.
    /// </summary>
    [Fact]
    public void IndexReturnsViewForNoRole()
    {
        // Arrange
        var controller = HomeControllerLogger();
        CreateUserSubstitute(controller, "dummyUserId");

        // Ensure no roles are set to true.
        _userManager.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(false);

        // Act
        var result = controller.Index().Result; // Get the result synchronously.

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    /// <summary>
    /// Verifies that the Privacy action of the HomeController returns a ViewResult with a null view name.
    /// </summary>
    [Fact]
    public void PrivacyReturnsPrivacyView()
    {
        //arrange
        var controller = HomeControllerLogger();
        
        //act 
        var result = controller.Privacy();
        var viewResult = result as ViewResult;
        
        //assert
        Assert.Null(viewResult!.ViewName);
    }
}