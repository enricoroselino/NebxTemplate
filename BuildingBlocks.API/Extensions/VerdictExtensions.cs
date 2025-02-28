using BuildingBlocks.API.Verdict;
using Microsoft.AspNetCore.Http;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Extensions;

public static class VerdictExtensions
{
    private const string ContentType = "application/json";

    public static IResult ToResult(this IVerdict verdict, HttpContext context)
    {
        return verdict.Status switch
        {
            VerdictStatus.Ok => verdict is Verdict.Verdict ? Results.Ok() : Results.Ok(verdict.GetValue()),
            VerdictStatus.NoContent => Results.NoContent(),
            VerdictStatus.Created => Results.Created("", verdict.GetValue()),
            VerdictStatus.NotFound => NotFound(verdict, context),
            VerdictStatus.Forbidden => Forbidden(verdict, context),
            VerdictStatus.Unauthorized => Unauthorized(verdict, context),
            VerdictStatus.BadRequest => BadRequest(verdict, context),
            VerdictStatus.UnprocessableEntity => UnprocessableEntity(verdict, context),
            VerdictStatus.Conflict => Conflict(verdict, context),
            VerdictStatus.InternalError => ErrorResult(verdict),
            _ => throw new NotSupportedException("Verdict to result is not supported.")
        };
    }

    private static IResult UnprocessableEntity(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status403Forbidden;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        return Results.UnprocessableEntity(errorResponse);
    }

    private static IResult Forbidden(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status403Forbidden;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        return Results.Json(errorResponse, contentType: ContentType, statusCode: statusCode);
    }

    private static IResult NotFound(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status404NotFound;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        return Results.NotFound(errorResponse);
    }

    private static IResult BadRequest(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status400BadRequest;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        if (verdict.ValidationErrors is not null) errorResponse.AddErrors(verdict.ValidationErrors);
        return Results.BadRequest(errorResponse);
    }

    private static IResult Conflict(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status409Conflict;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        return Results.Conflict(errorResponse);
    }

    private static IResult Unauthorized(IVerdict verdict, HttpContext context)
    {
        const int statusCode = StatusCodes.Status401Unauthorized;
        var errorResponse = ErrorResponse.Create(verdict.ErrorMessage, context, statusCode);
        return Results.Json(errorResponse, contentType: ContentType, statusCode: statusCode);
    }

    private static IResult ErrorResult(IVerdict verdict)
    {
        // exceptional event should have exceptional handling
        // this should be handled by GlobalExceptionHandler
        throw new InternalServerException(verdict.ErrorMessage);
    }
}