using Accommodations.Application.Behaviour;
using Autofac;
using Infrastructure;
using Infrastructure.DomainEventDispatching;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Accommodations.Infrastructure.Configuration.Processing;

public class ProcessingModule :  Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventDispatcher>()
            .As<IDomainEventDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventAccessor>()
            .As<IDomainEventAccessor>()
            .InstancePerLifetimeScope();
        
        builder.RegisterGeneric(typeof(TransactionalBehaviour<,>))
            .As(typeof(IPipelineBehavior<,>))
            .InstancePerLifetimeScope();
        
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<DomainEventNotificationMapper>()
            .As<IDomainEventNotificationMapper>()
            .WithParameter("domainNotificationsMap", new BiDictionary<string, Type>())
            .SingleInstance();
    }
}