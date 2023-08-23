using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
        {
            try
            {
                return await action();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception raised");

                ErrorResponse errorResponse = new()
                {
                    Code = ErrorCode.InternalServerError,
                    Message = "Unhandled error"
                };

                return ToActionResult(errorResponse);
            }
        }

        protected IActionResult ToActionResult(ErrorResponse errorResponse)
        {
            return StatusCode((int)errorResponse.Code / 100, errorResponse);
        }
    }

}