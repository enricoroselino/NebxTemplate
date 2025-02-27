using Shared.Models.ValueObjects;

namespace Shared.Models.Responses;

public class MetaBuilder
{
    private readonly Meta _meta = new();

    public MetaBuilder AddPagination(Pagination pagination, int totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);
        _meta.AddPagination(pagination, totalCount, totalPages);
        return this;
    }

    public MetaBuilder AddWarnings(Dictionary<string, string> warnings)
    {
        _meta.AddWarnings(warnings);
        return this;
    }

    public Meta Build()
    {
        return _meta;
    }
}