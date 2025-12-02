using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Models.Account;
using NRLObstacleReporting.Repositories;
using NSubstitute;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Controllers;

[TestSubject(typeof(AdminController))]
public class AdminControllerTests
{
    private UserManager<IdentityUser> _userManager;
    private ILogger<AccountController> _logger;
    private IMapper _mapper;
    private IAdminRepository _adminRepository;
    
    /// <summary>
    /// Creates an instance of the <see cref="AdminController"/> for tests.
    /// </summary>
    /// <returns>
    /// A new <see cref="AdminController"/> instance initialized with mocked dependencies.
    /// </returns>
    private AdminController CreateAdminController()
    {
        var userStore = Substitute.For<IUserStore<IdentityUser>>();
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
        
        _userManager = userManager;
        _logger = Substitute.For<ILogger<AccountController>>();
        _mapper = Substitute.For<IMapper>();
        _adminRepository = Substitute.For<IAdminRepository>();
        
        var controller = new AdminController(_userManager, _logger, _mapper, _adminRepository);
        
        return controller;
    }

    /// <summary>
    /// Tests the <see cref="AdminController.AdminIndex"/> method to ensure it returns a valid <see cref="ViewResult"/>.
    /// </summary>
    /// <remarks>
    /// Verifies that the result object is not null, is of type <see cref="ViewResult"/>, and that the <see cref="ViewResult.ViewName"/> is null, indicating the default view is used.
    /// </remarks>
    [Fact]
    public void AdminIndex_ReturnsViewResult()
    {
        // Arrange
        var controller = CreateAdminController();

        // Act
        var result = controller.AdminIndex();
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult!.ViewName);
    }

    /// <summary>
    /// Tests the <see cref="AdminController.Register()"/> method to ensure it returns a valid <see cref="ViewResult"/>.
    /// </summary>
    /// <remarks>
    /// Verifies that the result object is not null, is of type <see cref="ViewResult"/>, and that the <see cref="ViewResult.ViewName"/> is null, indicating the default view is used.
    /// </remarks>
    [Fact]
    public void RegisterGET_ReturnsViewResult()
    {
        // Arrange
        var controller = CreateAdminController();

        // Act
        var result = controller.Register();
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult!.ViewName);
    }
    
    /// <summary>
    /// Validates that adding an error through <see cref="Controller.ModelState"/> correctly associates the
    /// error message with the specified key in the model state.
    /// </summary>
    /// <remarks>
    /// This method verifies that the error key is present in the model state and that the associated error message matches the expected value.
    /// </remarks>
    [Fact]
    public void AddError_AddsErrorToModelState()
    {
        // Arrange
        var controller = CreateAdminController();
        const string errorMessage = "Test error message";
        const string errorKey = "TestErrorKey";

        // Act
        controller.ModelState.AddModelError(errorKey, errorMessage);
        var modelState = controller.ModelState;
        
        // Assert
        var errorMessages = new List<string>();
        foreach (var err in modelState[errorKey]!.Errors) // Suck all error messages to list, so we can check if expected msg is there
        {
            errorMessages.Add(err.ErrorMessage);
        }
        
        Assert.True(modelState.ContainsKey(errorKey));
        Assert.Contains(errorMessage, errorMessages);
    }

    /// <summary>
    /// Tests the <see cref="AdminController.ManageUsers"/> method to verify that it returns a <see cref="ViewResult"/>
    /// containing a collection of users retrieved from the database and mapped to the appropriate view model. ASSUMES REPO BEHAVES
    /// </summary>
    [Fact]
    public void ManageUsers_ReturnsViewResultWithUsersFromDatabase()
    {
        // Arrange
        var controller = CreateAdminController();
        
        var mockUsers = new List<ViewUserRoleDto>
        {
            new ViewUserRoleDto
            {
                UserId = "1",
                UserName = "User1",
                RoleId = "Pilot"
            },
            new ViewUserRoleDto
            {
                UserId = "2",
                UserName = "User2",
                RoleId = "Registrar",
            },
            new ViewUserRoleDto
            {
                UserId = "3",
                UserName = "User3",
                RoleId = "Administrator"
            }

        }.AsEnumerable();
        
        _adminRepository.GetAllUsersAsync().Returns(mockUsers);
        var mappedUsers = _mapper.Map<IEnumerable<UserViewModel>>(mockUsers); // map users to viewmodel
        
        // Act
        var result = controller.ManageUsers().Result;
        var viewResult = result as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(viewResult!.ViewName);
        Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.Model as IEnumerable<UserViewModel>);
        Assert.Equal(mappedUsers, viewResult.Model);
    }

    /// <summary>
    /// Tests the <see cref="AdminController.Register()"/> method when the model state is invalid.
    /// </summary>
    /// <remarks>
    /// Verifies that the method returns a <see cref="ViewResult"/> with correct model, and the model state contains the expected error messages.
    /// </remarks>
    [Fact]
    public void RegisterPOST_InvalidModelState_ReturnsViewResult()
    {
        // Arrange
        var controller = CreateAdminController();
        
        const string errorMessage = "Test error message";
        const string errorKey = "TestErrorKey";
        
        controller.ModelState.AddModelError(errorKey, errorMessage);

        // Act
        var result = controller.Register(new RegisterViewModel()).Result;
        var viewResult = result as ViewResult;
        var modelState = controller.ModelState;
        var model = viewResult!.Model;

        // Assert
        var errorMessages = new List<string>();
        foreach (var err in modelState[errorKey]!.Errors) // Suck all error messages to list, so we can check if expected msg is there
        {
            errorMessages.Add(err.ErrorMessage);
        }
        
        Assert.True(modelState.ContainsKey(errorKey));
        Assert.Contains(errorMessage, errorMessages);
        Assert.IsType<RegisterViewModel>(model);
        Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult!.ViewName);
        Assert.False(controller.ModelState.IsValid);
    }

    /// <summary>
    /// Verifies that when the account creation process fails during a registration attempt,
    /// the <see cref="AdminController.Register(RegisterViewModel)"/> method returns a <see cref="ViewResult"/>.
    /// </summary>
    /// <remarks>
    /// Ensures that the method returns the appropriate view result while maintaining the invalid model state,
    /// allowing error information to be displayed to the user.
    /// </remarks>
    [Fact]
    public void RegisterPOST_CreateAccountFailure_ReturnsViewResult()
    {
        // Arrange
        var controller = CreateAdminController();
        
        const string errorMessage = "Account creation failure";
        
        var registerViewModel = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = "Test@123",
            ConfirmPassword = "Test@123",
            Role = RegisterViewModel.UserRole.Registrar
        };

        _userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(new IdentityError { Description = errorMessage }));

        // Act
        var result = controller.Register(registerViewModel).Result;
        var viewResult = result as ViewResult;
        var model = viewResult!.Model;
        var modelState = controller.ModelState;

        // Assert
        var errorMessages = new List<string>();
        foreach (var err in modelState[""]!.Errors) // Suck all error messages to list, so we can check if expected msg is there
        {
            errorMessages.Add(err.ErrorMessage);
        }
        
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.IsType<RegisterViewModel>(model);
        Assert.Null(viewResult.ViewName);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(errorMessage, errorMessages);
    }
    
    /// <summary>
    /// Verifies that when the account creation process succeeds during a registration attempt,
    /// the <see cref="AdminController.Register(RegisterViewModel)"/> method redirects to the <see cref="AdminController.AdminIndex"/> action.
    /// </summary>
    [Fact]
    public void RegisterPOST_CreateAccountSuccess_RedirectsToAdminIndex()
    {
        // Arrange
        var controller = CreateAdminController();

        var registerViewModel = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = "Test@123",
            ConfirmPassword = "Test@123",
            Role = RegisterViewModel.UserRole.Pilot
        };

        _userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);

        // Act
        var result = controller.Register(registerViewModel).Result;
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AdminIndex", redirectResult!.ActionName);
        Assert.Null(redirectResult.ControllerName);
    }
}