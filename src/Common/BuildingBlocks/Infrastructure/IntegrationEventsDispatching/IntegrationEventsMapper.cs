namespace Infrastructure.IntegrationEventsDispatching;

public class IntegrationEventsMapper : IIntegrationEventsMapper
{
    private readonly BiDictionary<string, Type> _integrationEventsMaping;

    public IntegrationEventsMapper(BiDictionary<string, Type> integrationEventsMaping)
    {
        _integrationEventsMaping = integrationEventsMaping;
    }


    public string GetName(Type type)
    {
        return _integrationEventsMaping.TryGetBySecond(type, out var name) ? name 
            : throw new KeyNotFoundException($"The integration event type '{type.FullName}' is not registered in the mapping.");
    }

    public Type GetType(string name)
    {
        return _integrationEventsMaping.TryGetByFirst(name, out var type) ? type 
            : throw new KeyNotFoundException($"The integration event name '{name}' is not registered in the mapping.");
    }
}