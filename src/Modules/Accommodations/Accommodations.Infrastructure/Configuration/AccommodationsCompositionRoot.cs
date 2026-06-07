
using Autofac;

namespace Accommodations.Infrastructure.Configuration;

internal static class AccommodationsCompositionRoot
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

    internal static ILifetimeScope InfiniteScope()
    {
        return BeginLifetimeScope();
    }
}