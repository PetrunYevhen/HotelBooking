namespace Infrastructure.DomainEventDispatching;

public interface IDomainEventNotificationMapper
{
        string GetName(Type type);
        Type GetType(string name);
}