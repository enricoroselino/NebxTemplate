using BuildingBlocks.API.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API;

internal class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var message = exception switch
        {
            UnauthorizedException => exception.Message,
            DomainException => exception.Message,
            ValidationException => "One or more validation errors occurred.",
            _ => "An unexpected error occurred while processing your request."
        };

        var statusCode = exception switch
        {
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            DomainException => StatusCodes.Status422UnprocessableEntity,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = ErrorResponse.Create(message, httpContext, statusCode);

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .ToDictionary(c => c.PropertyName, c => c.ErrorMessage);

            errorResponse.AddErrors(errors);
        }

        _logger.LogError(exception,
            "Request failed with status code {StatusCode}. Exception: {ExceptionType} - {ExceptionMessage}. RequestId: {RequestId}, Path: {RequestPath}, Method: {RequestMethod};",
            statusCode, exception.GetType().Name, exception.Message, httpContext.TraceIdentifier,
            httpContext.Request.Method, httpContext.Request.Path);

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }
}