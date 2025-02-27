using System.Text.Json.Serialization;
using Shared.Models.Exceptions;

namespace Shared.Models.Responses;

public class Meta
{
    internal Meta()
    {
    }

    [JsonPropertyOrder(1)] public int? Page { get; private set; }
    [JsonPropertyOrder(2)] public int? PageSize { get; private set; }
    [JsonPropertyOrder(3)] public int? TotalCount { get; private set; }
    [JsonPropertyOrder(4)] public int? TotalPages { get; private set; }
    public Dictionary<string, string>? Warnings { get; private set; }

    internal void AddPagination(Pagination pagination, int totalCount, int totalPages)
    {
        if (pagination.Page > 1 && pagination.Page > totalPages)
        {
            throw new DomainException("Page must be less than the available total pages.");
        }

        Page = pagination.Page;
        PageSize = pagination.PageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }

    internal void AddWarnings(Dictionary<string, string> warnings)
    {
        Warnings = warnings;
    }
}