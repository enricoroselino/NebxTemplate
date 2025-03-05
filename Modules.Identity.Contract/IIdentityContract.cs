﻿using Modules.Identity.Contract.Dtos;

namespace Modules.Identity.Contract;

public interface IIdentityContract
{
    public Task<bool> IsUserExists(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        CancellationToken ct = default);

    public Task<UserDto?> GetUser(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        CancellationToken ct = default);
}