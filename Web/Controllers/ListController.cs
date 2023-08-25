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

public class ListController : Controller
{
    private readonly IMediator _mediator;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<ListController> _logger;

    public ListController(IMediator mediator,
        ITokenHandler tokenHandler,
        ILogger<ListController> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _tokenHandler = tokenHandler;
    }

    public async Task<IActionResult> ListTasks(int id, CancellationToken cancellationToken)
    {
        if (!IsAuthorized(out int _))
        {
            return Redirect("signin");
        }

        GetToDoTasksByListIdQuery query = new() { ToDoListId = id };
        GetToDoTasksByListIdQueryResult result = await _mediator.Send(query, cancellationToken);

        return View(result.ToDoListTasksPage);
    }

    public async Task<IActionResult> MyLists(CancellationToken cancellationToken)
    {
        if (!IsAuthorized(out int userId))
        {
            return RedirectToAction("signin", "home");
        }

        GetToDoListsByUserIdQuery query = new() { UserId = userId };
        GetToDoListsByUserIdQueryResult result = await _mediator.Send(query, cancellationToken);

        return View(result.ToDoLists);
    }

    public IActionResult CreateNewList()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewList([FromForm] ToDoList toDoList, CancellationToken cancellationToken)
    {
        if (!IsAuthorized(out int userId))
        {
            return Redirect("signin");
        }

        CreateToDoListCommand command = new()
        {
            UserId = userId,
            ToDoListName = toDoList.Name,
            ToDoListDescription = toDoList.Description
        };
        CreateToDoListCommandResult result = await _mediator.Send(command, cancellationToken);

        return Redirect("MyLists");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool IsAuthorized(out int userId)
    {
        if (HttpContext.Request.Cookies.TryGetValue("jwt", out string token))
        {
            return _tokenHandler.Validate(token, out userId);
        }

        userId = -1;
        return false;
    }
}
