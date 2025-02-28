using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingBlocks.API.Services;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
            v => v, // Store as is in the database (no conversion on write)
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Convert to UTC when reading from the database
        )
    {
    }
}