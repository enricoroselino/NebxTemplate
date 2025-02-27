using Microsoft.AspNetCore.Http;
using Shared.Models.Responses;

namespace BuildingBlocks.API.Extensions;

[Serializable]
public class ErrorResponse : ErrorBase
{
    private ErrorResponse() : base()
    {
    }
    
    public static ErrorResponse Create(string message, HttpContext context, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return new ErrorResponse()
        {
            StatusCode = statusCode,
            Message = message,
            RequestId = context.TraceIdentifier,
            Path = context.Request.Path,
        };
    }

    public void AddErrors(Dictionary<string, string> errors) => Errors = errors;
}