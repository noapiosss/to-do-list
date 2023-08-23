using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Domain.Base;
using Domain.Database;
using Domain.Helpers.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Commands
{
    public class CreateUserCommand : IRequest<CreateUserCommandResult>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class CreateUserCommandResult
    {
        public bool Success { get; init; }
        public bool UsernameIsAlreadyInUse { get; init; }
        public bool EmailIsAlreadyInUse { get; init; }
    }

    internal class CreateUserCommandHandler : BaseHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly ToDoListDbContext _dbContext;
        private readonly IPasswordHelper _passwordHelper;

        public CreateUserCommandHandler(ToDoListDbContext dbContext,
            IPasswordHelper passwordHelper,
            ILogger<CreateUserCommandHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
            _passwordHelper = passwordHelper;
        }

        protected override async Task<CreateUserCommandResult> HandleInternal(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool usernameIsAlreadyInUse = await _dbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
            bool emailIsAlreadyInUse = await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (usernameIsAlreadyInUse || emailIsAlreadyInUse)
            {
                return new()
                {
                    Success = false,
                    UsernameIsAlreadyInUse = usernameIsAlreadyInUse,
                    EmailIsAlreadyInUse = emailIsAlreadyInUse
                };
            }

            User user = new()
            {
                Username = request.Username,
                Email = request.Email,
                Password = _passwordHelper.ComputeSha256Hash(request.Password)
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new()
            {
                Success = true,
                UsernameIsAlreadyInUse = false,
                EmailIsAlreadyInUse = false
            };
        }
    }
}