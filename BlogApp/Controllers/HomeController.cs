using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    [HttpGet]
    [Authorize]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult LoginPartial()
    {
        return PartialView("_LoginPartial");
    }

    public IActionResult Index()
    {
        if (User.Identity!.IsAuthenticated)
        {
        
            return RedirectToAction("Index", "Blog");
        }
        else
        {
            return View();
        }
    }
    [Route("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }
    [Route("Iletisim")]
    public IActionResult Iletisim()
    {
        return View();
    }
    [Route("Hakkimizda")]
    public IActionResult Hakkimizda()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
