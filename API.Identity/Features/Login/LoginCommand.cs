using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace API.Identity.Features.Login;

public record LoginCommand() : ICommand<Response<LoginResponse>>;
