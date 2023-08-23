using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Contracts.DTO;
using Domain.Base;
using Domain.Commands;
using Domain.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Queries
{
    public class GetToDoTasksByListIdQuery : IRequest<GetToDoTasksByListIdQueryResult>
    {
        public int ToDoListId { get; init; }
    }

    public class GetToDoTasksByListIdQueryResult
    {
        public ICollection<ToDoTaskDTO> ToDoTasks { get; init; }
    }

    internal class GetToDoTasksByListIdQueryHandler : BaseHandler<GetToDoTasksByListIdQuery, GetToDoTasksByListIdQueryResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public GetToDoTasksByListIdQueryHandler(ToDoListDbContext dbContext,
            ILogger<GetToDoTasksByListIdQueryHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<GetToDoTasksByListIdQueryResult> HandleInternal(GetToDoTasksByListIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.ToDoLists.AnyAsync(u => u.Id == request.ToDoListId, cancellationToken))
            {
                return new()
                {
                    ToDoTasks = null
                };
            }

            List<ToDoTaskDTO> toDoTaskDTOs = await _dbContext.ToDoTasks
                .Where(tdt => tdt.ToDoListId == request.ToDoListId)
                .Select(tdl => new ToDoTaskDTO() 
                {
                    Id = tdl.Id,
                    Name = tdl.Name,
                    Description = tdl.Description,
                    CreationDateTime = tdl.CreationDateTime,
                    CompletionDateTime = tdl.CompletionDateTime
                })
                .ToListAsync(cancellationToken);

            return new()
            {
                ToDoTasks = toDoTaskDTOs
            };
        }
    }
}