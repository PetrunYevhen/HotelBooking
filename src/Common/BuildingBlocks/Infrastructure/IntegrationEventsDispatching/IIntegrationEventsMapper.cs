namespace Infrastructure.IntegrationEventsDispatching;

public interface IIntegrationEventsMapper
{
    string GetName(Type type);
    Type GetType(string name);
}