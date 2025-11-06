using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NRLObstacleReporting.Models.Account;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;

    public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
    }
    
    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
    
    [HttpGet]
    public IActionResult AdminIndex()
    {
        return View();
    }
    
    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true, LockoutEnabled = false, LockoutEnd = null };
        var createAccResult = await _userManager.CreateAsync(user, model.Password);
        
        if (!createAccResult.Succeeded)
        {
            AddErrors(createAccResult);
            return View(model);
        }
        
        _logger.LogInformation(3, "User created a new account with password.");
        
        await _userManager.AddToRoleAsync(user, model.Role.ToString());
        
        return RedirectToAction("AdminIndex");
    }
}