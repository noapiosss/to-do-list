using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Domain.Base;
using Domain.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Commands
{
    public class CreateToDoTaskCommand : IRequest<CreateToDoTaskCommandResult>
    {
        public int ToDoListId { get; init; }
        public string ToDoTaskName { get; init; }
        public string ToDoTaskDescription { get; init; }
    }

    public class CreateToDoTaskCommandResult
    {
        public int ToDoTaskId { get; init; }
    }

    internal class CreateToDoTaskCommandHandler : BaseHandler<CreateToDoTaskCommand, CreateToDoTaskCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public CreateToDoTaskCommandHandler(ToDoListDbContext dbContext,
            ILogger<CreateToDoTaskCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<CreateToDoTaskCommandResult> HandleInternal(CreateToDoTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.ToDoLists.AnyAsync(tdl => tdl.Id == request.ToDoListId, cancellationToken))
            {
                return new()
                {
                    ToDoTaskId = -1
                };
            }

            ToDoTask toDoTask = new()
            {
                Name = request.ToDoTaskName,
                Description = request.ToDoTaskDescription,
                CreationDateTime = DateTime.UtcNow,
                ToDoListId = request.ToDoListId
            };

            await _dbContext.ToDoTasks.AddAsync(toDoTask, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new()
            {
                ToDoTaskId = toDoTask.Id
            };
        }
    }
}