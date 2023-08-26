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
        private readonly IUserService _userService;

        public ListsHelperController(IMediator mediator,
            IUserService userService,
            ILogger<ListsHelperController> logger) : base(logger)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpDelete]
        public Task<IActionResult> DeleteLisById([FromBody] DeleteToDoListRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!_userService.IsAuthorized(out int userId))
                {
                    ErrorResponse errorResponse = new()
                    {
                        Code = ErrorCode.Unauthorized,
                        Message = "Unathorized"
                    };

                    return ToActionResult(errorResponse);
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
                if (!_userService.IsAuthorized(out int userId))
                {
                    ErrorResponse errorResponse = new()
                    {
                        Code = ErrorCode.Unauthorized,
                        Message = "Unathorized"
                    };

                    return ToActionResult(errorResponse);
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

        [HttpDelete("tasks")]
        public Task<IActionResult> DeleteTask([FromBody] DeleteToDoTaskRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!_userService.IsAuthorized(out int userId))
                {
                    ErrorResponse errorResponse = new()
                    {
                        Code = ErrorCode.Unauthorized,
                        Message = "Unathorized"
                    };

                    return ToActionResult(errorResponse);
                }

                DeleteToDoTaskCommand command = new() { ToDoTaskId = request.TaskId };
                DeleteToDoTaskCommandResult result = await _mediator.Send(command, cancellationToken);

                return result.Success ? Ok() : BadRequest(result);

            }, cancellationToken);
        }

        [HttpPut("tasks")]
        public Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!_userService.IsAuthorized(out int userId))
                {
                    ErrorResponse errorResponse = new()
                    {
                        Code = ErrorCode.Unauthorized,
                        Message = "Unathorized"
                    };

                    return ToActionResult(errorResponse);
                }

                UpdateToDoTaskStatusCommand command = new() { ToDoTaskId = request.TaskId };
                UpdateToDoTaskStatusCommandResult result = await _mediator.Send(command, cancellationToken);

                return result.Success ? Ok(result) : BadRequest(result);

            }, cancellationToken);
        }
    }
}