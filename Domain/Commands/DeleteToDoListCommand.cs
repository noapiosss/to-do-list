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
    public class DeleteToDoListCommand : IRequest<DeleteToDoListCommandResult>
    {
        public int ToDoListId { get; init; }
    }

    public class DeleteToDoListCommandResult
    {
        public bool Success { get; init; }
        public bool ToDoListExists { get; init; }
    }

    internal class DeleteToDoListCommandHandler : BaseHandler<DeleteToDoListCommand, DeleteToDoListCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public DeleteToDoListCommandHandler(ToDoListDbContext dbContext,
            ILogger<DeleteToDoListCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<DeleteToDoListCommandResult> HandleInternal(DeleteToDoListCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.ToDoLists.AnyAsync(tdl => tdl.Id == request.ToDoListId, cancellationToken))
            {
                return new()
                {
                    Success = false,
                    ToDoListExists = false
                };
            }

            ToDoList toDoList = new() { Id = request.ToDoListId };

            _ = _dbContext.ToDoLists.Attach(toDoList);
            _ = _dbContext.ToDoLists.Remove(toDoList);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
        
            return new()
            {
                Success = true,
                ToDoListExists = true
            };
        }
    }
}