using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace API.Features.Login;

public record LoginCommand() : ICommand<Response<LoginResponse>>;
