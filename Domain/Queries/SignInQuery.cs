using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Domain.Base;
using Domain.Database;
using Domain.Helpers.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Queries
{
    public class SignInQuery : IRequest<SignInQueryResult>
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }

    public class SignInQueryResult
    {
        public bool Success { get; init; }
        public bool LoginExists { get; init; }
        public bool PasswordIsCorrect { get; init; }
        public int UserId { get; init; }
    }

    internal class SignInQueryHandler : BaseHandler<SignInQuery, SignInQueryResult>
    {
        private readonly ToDoListDbContext _dbContext;
        private readonly IPasswordHelper _passwrodHelper;

        public SignInQueryHandler(ToDoListDbContext dbContext,
            IPasswordHelper passwordHelper,
            ILogger<SignInQueryHandler> logger) : base(logger)
        {
            _dbContext = dbContext;
            _passwrodHelper = passwordHelper;
        }

        protected override async Task<SignInQueryResult> HandleInternal(SignInQuery request, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Login || u.Email == request.Login, cancellationToken);

            string hashedPassword = _passwrodHelper.ComputeSha256Hash(request.Password);

            return new()
            {
                Success = user is not null && !string.IsNullOrEmpty(user.Password) && user.Password == hashedPassword,
                LoginExists = user is not null,
                PasswordIsCorrect = user is not null && !string.IsNullOrEmpty(user.Password) && user.Password == hashedPassword,
                UserId = user is not null && !string.IsNullOrEmpty(user.Password) && user.Password == hashedPassword ? user.Id : -1
            };
        }
    }
}