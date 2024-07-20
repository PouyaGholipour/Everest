using DomainServices.Exception;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context: context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, message: "Exception occured:{Message}", exception.Message);
                var exceptionDetails = GetExeptionDetails(exception);
                var problemDetails = new ProblemDetails
                {
                    Status = exceptionDetails.Status,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail,
                    Type = exceptionDetails.Type,
                };
                
                if(exceptionDetails.Errors is not null)
                    problemDetails.Extensions["Errors"] = exceptionDetails.Errors;

                context.Response.StatusCode = exceptionDetails.Status;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        private static ExceptionDetails GetExeptionDetails(Exception exception)
        {
            return exception switch
            {
                ServiceException serviceException => new ExceptionDetails(
                    Status: StatusCodes.Status400BadRequest,
                    Type: serviceException.Type,
                    Title: serviceException.Title,
                    Detail: serviceException.Detail,
                    Errors: serviceException.Errors
                    ),
                    _ => new ExceptionDetails(
                        Status: StatusCodes.Status102Processing,
                        Type: "OperationFailed",
                        Title: "Operation failed",
                        Detail: "Operation failed",
                        Errors: null
                        )
            };
        }

        internal record ExceptionDetails
        (
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object>? Errors
        );
    }
}
