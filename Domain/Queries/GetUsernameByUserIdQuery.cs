using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Queries
{
    public class GetUsernameByUserIdQuery : IRequest<GetUsernameByUserIdQueryResult>
    {
        public int UserId { get; init; }
    }

    public class GetUsernameByUserIdQueryResult
    {
        public string Username { get; set; }
    }

    internal class GetUsernameByUserIdQueryHandler : BaseHandler<GetUsernameByUserIdQuery, GetUsernameByUserIdQueryResult>
    {
        private readonly ToDoListDbContext _dbContext;

        public GetUsernameByUserIdQueryHandler(ToDoListDbContext dbContext,
            ILogger<GetUsernameByUserIdQueryHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        protected override async Task<GetUsernameByUserIdQueryResult> HandleInternal(GetUsernameByUserIdQuery request, CancellationToken cancellationToken)
        {
            return new()
            {
                Username = (await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken))?.Username
            };
        }
    }
}