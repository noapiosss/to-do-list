using System.Threading;
using System.Threading.Tasks;
using Contracts.Http;
using Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Route("api/lists")]
    public class ListsHelperController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ITokenHandler _tokenHandler;

        public ListsHelperController(IMediator mediator,
            ITokenHandler tokenHandler,
            ILogger<ListsHelperController> logger) : base(logger)
        {
            _mediator = mediator;
            _tokenHandler = tokenHandler;
        }

        [HttpDelete]
        public Task<IActionResult> DeleteLisById([FromBody] DeleteToDoListRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!IsAuthorized(out int _))
                {

                }

                DeleteToDoListCommand command = new() { ToDoListId = request.ListId };
                DeleteToDoListCommandResult result = await _mediator.Send(command, cancellationToken);

                return result.Success ? Ok() : BadRequest();

            }, cancellationToken);
        }

        [HttpPost("tasks")]
        public Task<IActionResult> AddTaskToList([FromBody] AddTaskRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!IsAuthorized(out int _))
                {

                }

                CreateToDoTaskCommand command = new()
                {
                    ToDoListId = request.ListId,
                    ToDoTaskName = request.TaskName,
                    ToDoTaskDescription = request.TaskDescription
                };
                CreateToDoTaskCommandResult result = await _mediator.Send(command, cancellationToken);

                return result.ToDoTaskDTO is null ? BadRequest() : Ok(result.ToDoTaskDTO);

            }, cancellationToken);
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
}