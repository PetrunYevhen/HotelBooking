using Autofac;
using Infrastructure;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Mediation;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));
        
        builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        var mediatorOpenTypes = new List<Type>()
        {
            typeof(IRequestHandler<>),
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
        };
        
        foreach (var mediatorOpenType in mediatorOpenTypes)
        {
            builder.RegisterAssemblyTypes(Assemblies.Application, ThisAssembly)
                .AsClosedTypesOf(mediatorOpenType)
                .AsImplementedInterfaces()
                .FindConstructorsWith(new AllConstructorFinder());
        }

    }
}