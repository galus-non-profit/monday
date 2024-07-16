namespace Monday.WebApi.Handlers;

using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Monday.WebApi.Exceptions;

public class ExceptionsHandler : IExceptionHandler
{
    private const string DEFAULT_SEPARATOR = ", ";
    private const string DEFAULT_TITLE = "An error occurred";

    private readonly IProblemDetailsService problemDetailsService;

    public ExceptionsHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var details = string.Empty;

        if (exception is ValidationException validationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            details = string.Join(DEFAULT_SEPARATOR, validationException.Failures);
        }

        httpContext.Response.StatusCode = statusCode;

        return await this.problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = DEFAULT_TITLE,
                Detail = details,
            },
            Exception = exception,
        });
    }
}
