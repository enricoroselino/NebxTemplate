using BuildingBlocks.API.Models.DDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.API.Configurations.EFCore;

internal class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public SoftDeleteInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entities = GetDeletedEntities(eventData.Context);
        UpdateSoftDeleteProperties(entities);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entities = GetDeletedEntities(eventData.Context);
        UpdateSoftDeleteProperties(entities);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateSoftDeleteProperties(List<EntityEntry<ISoftDeletable>> entities)
    {
        var actionTime = _timeProvider.GetUtcNow().UtcDateTime;
        
        entities.ForEach(x =>
        {
            x.State = EntityState.Modified;
            x.Entity.DeletedOn = actionTime;
        });
    }

    private static List<EntityEntry<ISoftDeletable>> GetDeletedEntities(DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(x => x.State == EntityState.Deleted)
            .ToList();

        return entities;
    }
}