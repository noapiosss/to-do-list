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
    public class CreateToDoListCommand : IRequest<CreateToDoListCommandResult>
    {
        public int UserId { get; init; }
        public string ToDoListName { get; init; }
        public string ToDoListDescription { get; init; }
    }

    public class CreateToDoListCommandResult
    {
        public int ToDoListId { get; init; }
    }

    internal class CreateToDoListCommandHandler : BaseHandler<CreateToDoListCommand, CreateToDoListCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public CreateToDoListCommandHandler(ToDoListDbContext dbContext,
            ILogger<CreateToDoListCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<CreateToDoListCommandResult> HandleInternal(CreateToDoListCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
            {
                return new()
                {
                    ToDoListId = -1
                };
            }

            ToDoList toDoList = new()
            {
                Name = request.ToDoListName,
                Description = request.ToDoListDescription,
                CreationDateTime = DateTime.UtcNow,
                UserId = request.UserId
            };

            await _dbContext.ToDoLists.AddAsync(toDoList, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new()
            {
                ToDoListId = toDoList.Id
            };
        }
    }
}