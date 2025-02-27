using BuildingBlocks.API.Models.DDD;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.API.Configurations.EFCore;

internal sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // eventual consistency
        var saveResult = base.SavingChanges(eventData, result);
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return saveResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        // eventual consistency
        var saveResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await DispatchDomainEvents(eventData.Context);
        return saveResult;
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DequeueEvents())
            .ToList();

        foreach (var domainEvent in domainEvents) await _mediator.Publish(domainEvent);
    }
}