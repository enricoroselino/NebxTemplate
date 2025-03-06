using Ardalis.Specification;
using BuildingBlocks.API.Models.DDD;
using Shared.Models.ValueObjects;

namespace BuildingBlocks.API.Models;

public sealed class PaginationSpecification<T> : Specification<T> where T : IEntity
{
    public PaginationSpecification(Pagination? pagination)
    {
        if (pagination is null) return;

        Query
            .Skip(pagination.Offset)
            .Take(pagination.PageSize);
    }
}