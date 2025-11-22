using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Models.Account;
using NRLObstacleReporting.Repositories;

namespace NRLObstacleReporting.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;
    private readonly IAdminRepository _adminRepository;

    public AdminController(UserManager<IdentityUser> userManager, ILogger<AccountController> logger, IMapper mapper,
        IAdminRepository adminRepository)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _adminRepository = adminRepository;
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
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
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

    [HttpGet]
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _adminRepository.GetAllUsers();
        var modelListDraft = _mapper.Map<IEnumerable<UserViewModel>>(users);
        
        return View(modelListDraft);
    }
}