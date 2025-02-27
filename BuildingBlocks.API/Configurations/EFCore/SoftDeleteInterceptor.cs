using BuildingBlocks.API.Models.DDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.API.Configurations.EFCore;

/// <summary>
/// Intercepts SaveChanges operations to implement soft delete functionality.
/// </summary>
internal class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftDeleteInterceptor"/> class.
    /// </summary>
    /// <param name="timeProvider">Provides the current time.</param>
    public SoftDeleteInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Called at the start of <see cref="DbContext.SaveChanges"/> to process soft deletions.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/>.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <returns>An <see cref="InterceptionResult{Int32}"/> that may modify the result of the operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entities = GetDeletedEntities(eventData.Context);
        UpdateSoftDeleteProperties(entities);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Called at the start of <see cref="DbContext.SaveChangesAsync"/> to process soft deletions asynchronously.
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
        var entities = GetDeletedEntities(eventData.Context);
        UpdateSoftDeleteProperties(entities);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the properties of entities marked for soft deletion.
    /// </summary>
    /// <param name="entities">A list of entities implementing <see cref="ISoftDeletable"/> that are marked as deleted.</param>
    private void UpdateSoftDeleteProperties(List<EntityEntry<ISoftDeletable>> entities)
    {
        var actionTime = _timeProvider.GetUtcNow().UtcDateTime;
        
        entities.ForEach(x =>
        {
            x.State = EntityState.Modified;
            x.Entity.DeletedOn = actionTime;
        });
    }

    /// <summary>
    /// Retrieves entities marked for deletion that implement the <see cref="ISoftDeletable"/> interface.
    /// </summary>
    /// <param name="context">The current <see cref="DbContext"/> instance.</param>
    /// <returns>A list of <see cref="EntityEntry{ISoftDeletable}"/> entries marked as deleted.</returns>
    private static List<EntityEntry<ISoftDeletable>> GetDeletedEntities(DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(x => x.State == EntityState.Deleted)
            .ToList();

        return entities;
    }
}