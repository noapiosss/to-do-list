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
    public class DeleteToDoTaskCommand : IRequest<DeleteToDoTaskCommandResult>
    {
        public int ToDoTaskId { get; init; }
    }

    public class DeleteToDoTaskCommandResult
    {
        public bool Success { get; init; }
        public bool ToDoTaskExists { get; init; }
    }

    internal class DeleteToDoTaskCommandHandler : BaseHandler<DeleteToDoTaskCommand, DeleteToDoTaskCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public DeleteToDoTaskCommandHandler(ToDoListDbContext dbContext,
            ILogger<DeleteToDoTaskCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<DeleteToDoTaskCommandResult> HandleInternal(DeleteToDoTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.ToDoTasks.AnyAsync(tdt => tdt.Id == request.ToDoTaskId, cancellationToken))
            {
                return new()
                {
                    Success = false,
                    ToDoTaskExists = false
                };
            }

            ToDoTask toDoTask = new() { Id = request.ToDoTaskId };

            _ = _dbContext.ToDoTasks.Attach(toDoTask);
            _ = _dbContext.ToDoTasks.Remove(toDoTask);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
        
            return new()
            {
                Success = true,
                ToDoTaskExists = true
            };
        }
    }
}