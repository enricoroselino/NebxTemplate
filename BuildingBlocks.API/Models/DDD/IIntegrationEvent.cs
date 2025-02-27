using MediatR;

namespace BuildingBlocks.API.Models.DDD;

public interface IIntegrationEvent : INotification
{
    public Guid EventId => Guid.CreateVersion7();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName!;
}