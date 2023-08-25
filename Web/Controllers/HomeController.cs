using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Domain.Commands;
using Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IMediator mediator,
        ITokenHandler tokenHandler,
        ILogger<HomeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _tokenHandler = tokenHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
