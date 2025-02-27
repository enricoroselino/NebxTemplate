using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Models.Interfaces;

namespace BuildingBlocks.API.Configurations.EFCore;

internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public TimeAuditInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

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