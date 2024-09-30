using contacts_app.Api.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace contacts_app.Api.Common.ExceptionHandlers
{
    public class ForbiddenExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ForbiddenExceptionHandler> _logger;

        public ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var type = exception.GetType();
            if (exception.GetType() != typeof(ForbiddenException))
            {
                return false;
            }

            var forbiddenException = exception as ForbiddenException;

            _logger.LogError(
           forbiddenException,
           "Exception occurred: {Message}",
           forbiddenException.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Detail = forbiddenException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}