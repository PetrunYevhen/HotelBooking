using Autofac;

namespace Rooms.Infrastructure.Configuration;

public static class RoomsCompositoryRoot 
{
    private static IContainer _container;
    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }
    
    internal static ILifetimeScope BeginLifetimeScope()
    {
        if (_container is null)
        {
            throw new InvalidOperationException("Container is not initialized.");
        }
        
        return _container.BeginLifetimeScope();
    }
    
    
}