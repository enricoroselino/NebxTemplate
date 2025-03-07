﻿namespace Modules.Identity.Application.Features.Authentication.RefreshToken;

public record RefreshTokenCommand(Guid UserId, Guid TokenId, string RefreshToken)
    : ICommand<Verdict<Response<RefreshTokenResponse>>>;