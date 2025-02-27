using Ardalis.GuardClauses;
using Shared.Models.Exceptions;

namespace Shared.Models;

public record TimeRange
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    public TimeRange(DateTime start, DateTime end)
    {
        Start = Guard.Against.OutOfSQLDateRange(start, nameof(start));
        End = Guard.Against.OutOfSQLDateRange(end, nameof(end));

        if (End < Start)
        {
            throw new DomainException("End date must not be earlier than start date.");
        }
    }

    public bool IsOverlapped(TimeRange timeRange)
    {
        return timeRange.Start <= Start && timeRange.End >= End;
    }
}