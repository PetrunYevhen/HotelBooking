using MediatR;

namespace BuildingBlock.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}