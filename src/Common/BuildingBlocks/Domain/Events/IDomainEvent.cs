using MediatR;

namespace BuildingBlock.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}