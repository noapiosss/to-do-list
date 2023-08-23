using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.DTO;
using Domain.Base;
using Domain.Commands;
using Domain.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Queries
{
    public class GetToDoListsByUserIdQuery : IRequest<GetToDoListsByUserIdQueryResult>
    {
        public int UserId { get; init; }
    }

    public class GetToDoListsByUserIdQueryResult
    {
        public ICollection<ToDoListDTO> ToDoLists { get; init; }
    }

    internal class GetToDoListsByUserIdQueryHandler : BaseHandler<GetToDoListsByUserIdQuery, GetToDoListsByUserIdQueryResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public GetToDoListsByUserIdQueryHandler(ToDoListDbContext dbContext,
            ILogger<GetToDoListsByUserIdQueryHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<GetToDoListsByUserIdQueryResult> HandleInternal(GetToDoListsByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
            {
                return new()
                {
                    ToDoLists = null
                };
            }

            List<ToDoListDTO> toDoListDTOs = await _dbContext.ToDoLists
                .Where(tdl => tdl.UserId == request.UserId)
                .Select(tdl => new ToDoListDTO() 
                {
                    Id = tdl.Id,
                    Name = tdl.Name,
                    Description = tdl.Description,
                    CreationDateTime = tdl.CreationDateTime,
                    CompletionDateTime = tdl.CompletionDateTime,
                    TasksCount = tdl.ToDoTasks.Count(),
                    CompletedTasksCount = tdl.ToDoTasks.Where(tdt => tdt.CompletionDateTime != null).Count()
                })
                .ToListAsync(cancellationToken);

            return new()
            {
                ToDoLists = toDoListDTOs
            };
        }
    }
}