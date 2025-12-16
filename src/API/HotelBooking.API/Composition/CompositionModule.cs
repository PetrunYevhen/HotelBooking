using Autofac;

namespace HotelBooking.API.Composition;

public class CompositionModule : Module
{

    protected override void Load(ContainerBuilder builder)
    {
        var assembly = typeof(CompositionModule).Assembly;
        
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("CompositionService"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}