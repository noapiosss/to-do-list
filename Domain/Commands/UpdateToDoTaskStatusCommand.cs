using System;
using System.Linq;
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
    public class UpdateToDoTaskStatusCommand : IRequest<UpdateToDoTaskStatusCommandResult>
    {
        public int ToDoTaskId { get; init; }
    }

    public class UpdateToDoTaskStatusCommandResult
    {
        public bool Success { get; init; }
        public bool ToDoTaskExists { get; init; }
    }

    internal class UpdateToDoTaskStatusCommandHandler : BaseHandler<UpdateToDoTaskStatusCommand, UpdateToDoTaskStatusCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public UpdateToDoTaskStatusCommandHandler(ToDoListDbContext dbContext,
            ILogger<UpdateToDoTaskStatusCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<UpdateToDoTaskStatusCommandResult> HandleInternal(UpdateToDoTaskStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.ToDoTasks.AnyAsync(tdt => tdt.Id == request.ToDoTaskId, cancellationToken))
            {
                return new()
                {
                    Success = false,
                    ToDoTaskExists = false
                };
            }

            ToDoTask toDoTask = await _dbContext.ToDoTasks.FirstAsync(tdt => tdt.Id == request.ToDoTaskId, cancellationToken);
            toDoTask.CompletionDateTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!await _dbContext.ToDoTasks
                .Where(tdt => tdt.ToDoListId == toDoTask.ToDoListId)
                .AnyAsync(tdt => tdt.CompletionDateTime != null, cancellationToken))
            {
                ToDoList toDoList = await _dbContext.ToDoLists.FirstAsync(tbl => tbl.Id == toDoTask.ToDoListId, cancellationToken);
                toDoList.CompletionDateTime = toDoTask.CompletionDateTime;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        
            return new()
            {
                Success = true,
                ToDoTaskExists = true
            };
        }
    }
}