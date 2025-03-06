using Ardalis.GuardClauses;
using Shared.Models.Exceptions;

namespace Modules.SharedKernel;

public record UserId
{
    public Guid Value { get; init; }

    public UserId(Guid value)
    {
        Value = Guard.Against.NullOrEmpty(value, nameof(value),
            exceptionCreator: () => new DomainException("Guid cannot be null or empty."));
    }
};