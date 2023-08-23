using System.Threading;
using System.Threading.Tasks;
using Contracts.Http;
using Domain.Commands;
using Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Route("api/session")]
    public class SessionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ITokenHandler _tokenHandler;        

        public SessionController(IMediator mediator,
            ITokenHandler tokenHandler,
            ILogger<SessionController> logger) : base(logger)
        {
            _mediator = mediator;
            _tokenHandler = tokenHandler;
        }

        [HttpPost("signup")]
        public Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                CreateUserCommand command = new()
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password
                };

                CreateUserCommandResult result = await _mediator.Send(command, cancellationToken);

                if (!result.Success)
                {
                    if (result.UsernameIsAlreadyInUse)
                    {
                        ErrorResponse errorResponse = new()
                        {
                            Code = ErrorCode.UsernameIsAlreadyInUse,
                            Message = "Username is already in use"
                        };

                        return ToActionResult(errorResponse);
                    }

                    if (result.EmailIsAlreadyInUse)
                    {
                        ErrorResponse errorResponse = new()
                        {
                            Code = ErrorCode.EmailIsAlreadyInUse,
                            Message = "Email is already in use"
                        };

                        return ToActionResult(errorResponse);
                    }
                }

                SignUpResponse signUpResponse = new()
                {
                    Success = true
                };

                return Ok(signUpResponse);

            }, cancellationToken);
        }

        [HttpPost("signin")]
        public Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                SignInQuery query = new()
                {
                    Login = request.Login,
                    Password = request.Password
                };

                SignInQueryResult result = await _mediator.Send(query, cancellationToken);

                if (!result.Success)
                {
                    if (!result.LoginExists)
                    {
                        ErrorResponse errorResponse = new()
                        {
                            Code = ErrorCode.UserNotFound,
                            Message = "No user with such username/email"
                        };

                        return ToActionResult(errorResponse);
                    }

                    if (!result.PasswordIsCorrect)
                    {
                        ErrorResponse errorResponse = new()
                        {
                            Code = ErrorCode.WrongPassword,
                            Message = "Wrong password"
                        };

                        return ToActionResult(errorResponse);
                    }
                }

                SignInResponse response = new()
                {
                    Token = _tokenHandler.GenerateToken(result.UserId),
                    ExpireAt = 2592000
                };

                return Ok(response);

            }, cancellationToken);
        }
    }
}