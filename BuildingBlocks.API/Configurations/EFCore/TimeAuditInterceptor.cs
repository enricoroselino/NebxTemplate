using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Models.Interfaces;

namespace BuildingBlocks.API.Configurations.EFCore;

/// <summary>
/// Intercepts SaveChanges operations to automatically update time audit fields
/// (CreatedOn and ModifiedOn) for entities implementing <see cref="ITimeAuditable"/>.
/// </summary>
internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeAuditInterceptor"/> class.
    /// </summary>
    /// <param name="timeProvider">Provides the current time.</param>
    public TimeAuditInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Called at the start of <see cref="DbContext.SaveChanges"/> to update audit timestamps.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/>.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <returns>An <see cref="InterceptionResult{Int32}"/> that may modify the result of the operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Called at the start of <see cref="DbContext.SaveChangesAsync"/> to update audit timestamps asynchronously.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/>.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask{InterceptionResult{Int32}}"/> that may modify the result of the operation.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Retrieves entities that are tracked for auditing.
    /// </summary>
    /// <param name="context">The current <see cref="DbContext"/> instance.</param>
    /// <returns>A list of <see cref="EntityEntry{ITimeAuditable}"/> entries that require audit updates.</returns>
    private static List<EntityEntry<ITimeAuditable>> GetAuditEntityEntries(DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ITimeAuditable>()
            .Where(x =>
                x.State == EntityState.Added ||
                x.State == EntityState.Modified ||
                x.HasChangedOwnedEntities())
            .ToList();

        return entities;
    }

    /// <summary>
    /// Updates the audit timestamps for the tracked entities.
    /// </summary>
    /// <param name="entities">A list of entities implementing <see cref="ITimeAuditable"/>.</param>
    private void UpdateTimeAuditProperties(List<EntityEntry<ITimeAuditable>> entities)
    {
        var actionTime = _timeProvider.GetUtcNow().UtcDateTime;
        
        entities.ForEach(x =>
        {
            if (x.State == EntityState.Added)
                x.Entity.CreatedOn = actionTime;

            if (x.State == EntityState.Modified || x.HasChangedOwnedEntities())
                x.Entity.ModifiedOn = actionTime;
        });
    }
}