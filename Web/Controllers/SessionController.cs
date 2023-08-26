using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers;

public class SessionController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<SessionController> _logger;

    public SessionController(IUserService userService,
        ILogger<SessionController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public IActionResult SignIn()
    {
        if (_userService.IsAuthorized(out int _))
        {
            return RedirectToAction("MyLists", "List");
        }

        return View();
    }

    public IActionResult SignUp()
    {
        if (_userService.IsAuthorized(out int _))
        {
            return RedirectToAction("MyLists", "List");
        }
        
        return View();
    }

    public IActionResult SignOut()
    {
        if (_userService.IsAuthorized(out int _))
        {
            Response.Cookies.Delete("jwt");
            return Redirect("SignIn");
        }
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
