using Application.Events;
using Autofac;
using Infrastructure.DomainEventDispatching;
using Infrastructure.UnitOfWork;
using MediatR;
using Payments.Application.Behaviour;

namespace Payments.Infrastructure.Configuration.Processing;

public class ProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventDispatcher>()
            .As<IDomainEventDispatcher>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<DomainEventAccessor>()
            .As<IDomainEventAccessor>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(TransactionalBehaviour<,>))
            .As(typeof(IPipelineBehavior<,>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(Assemblies.Application)
            .AsClosedTypesOf(typeof(IDomainEventNotification<>))
            .InstancePerDependency()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}