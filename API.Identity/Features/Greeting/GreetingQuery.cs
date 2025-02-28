using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace API.Identity.Features.Greeting;

public record GreetingQuery : IQuery<Response<string>>;
