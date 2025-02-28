using Microsoft.AspNetCore.Http;
using Shared.Models.Responses;

namespace BuildingBlocks.API.Extensions;

public static class ResponseExtension
{
    public static IResult ToResult(this Response response) => Results.Ok(response);
}