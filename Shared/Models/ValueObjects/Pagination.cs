using Ardalis.GuardClauses;
using Shared.Models.Exceptions;

namespace Shared.Models.ValueObjects;

public record Pagination
{
    public int Page { get; }
    public int PageSize { get; }
    public int Offset => (Page - 1) * PageSize;

    public const int DefaultSize = 10;
    private readonly HashSet<int> _permittedPageSizes = [10, 20, 50, 100, 200, 500, 1000];

    public Pagination(int page, int pageSize = DefaultSize)
    {
        _permittedPageSizes.Add(DefaultSize);
        if (!_permittedPageSizes.Contains(pageSize))
        {
            throw new DomainException($"Permitted page sizes are {string.Join(", ", _permittedPageSizes)}");
        }

        Page = Guard.Against.NegativeOrZero(page, nameof(page),
            exceptionCreator: () => new DomainException("Page must be greater than zero."));
        PageSize = Guard.Against.NegativeOrZero(pageSize, nameof(pageSize),
            exceptionCreator: () => new DomainException("Page size must be greater than zero."));
    }
}