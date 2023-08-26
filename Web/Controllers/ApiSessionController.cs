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
    public class ApiSessionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserService _userService;

        public ApiSessionController(IMediator mediator,
            ITokenHandler tokenHandler,
            IUserService userService,
            ILogger<ApiSessionController> logger) : base(logger)
        {
            _mediator = mediator;
            _tokenHandler = tokenHandler;
            _userService = userService;
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
                    Email = request.Email,
                    Password = request.Password
                };

                SignInQueryResult result = await _mediator.Send(query, cancellationToken);

                if (!result.Success)
                {
                    if (!result.UserExists)
                    {
                        ErrorResponse errorResponse = new()
                        {
                            Code = ErrorCode.UserNotFound,
                            Message = "No user with such email"
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

        [HttpGet("me")]
        public Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!_userService.IsAuthorized(out int userId))
                {
                    ErrorResponse errorResponse = new()
                    {
                        Code = ErrorCode.Unauthorized,
                        Message = "Unathorized"
                    };

                    return ToActionResult(errorResponse);
                }

                GetUsernameByUserIdQuery query = new() { UserId = userId };
                GetUsernameByUserIdQueryResult result = await _mediator.Send(query, cancellationToken);

                return Ok(result.Username);

            }, cancellationToken);
        }
        
    }
}