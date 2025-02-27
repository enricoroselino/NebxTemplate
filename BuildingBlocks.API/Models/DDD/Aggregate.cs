namespace BuildingBlocks.API.Models.DDD;

public interface IAggregate : IEntity
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    public List<IDomainEvent> DequeueEvents();
}

public interface IAggregate<TId> : IAggregate, IEntity<TId>;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public List<IDomainEvent> DequeueEvents()
    {
        var events = _domainEvents.ToList();
        ClearDomainEvents();
        return events;
    }
}