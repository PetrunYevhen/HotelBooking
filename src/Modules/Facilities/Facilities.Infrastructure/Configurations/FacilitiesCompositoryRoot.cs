using Autofac;

namespace Facilities.Infrastructure.Configurations;

public class FacilitiesCompositoryRoot
{
    private static IContainer _container;
    
    public static void SetContainer(IContainer container)
    {
        _container = container;
    }
    
    public static ILifetimeScope BeginLifetimeScope()
    {
        if (_container is null)
        {
            throw new InvalidOperationException("Container is not initialized.");
        }
        
        return _container.BeginLifetimeScope();
    }
}