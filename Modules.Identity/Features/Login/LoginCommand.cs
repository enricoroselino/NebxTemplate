using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Login;

public record LoginCommand(string Identifier, string Password) : ICommand<Verdict<Response<LoginResponse>>>;