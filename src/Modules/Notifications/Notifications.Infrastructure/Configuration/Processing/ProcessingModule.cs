using Autofac;
using Infrastructure.DomainEventDispatching;
using Infrastructure.UnitOfWork;
using MediatR;
using Notifications.Application.Behaviour;

namespace Notifications.Infrastructure.Configuration.Processing;

public class ProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(TransactionalBehaviour<,>))
            .As(typeof(IPipelineBehavior<,>))
            .InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        

        builder.RegisterType<NullDomainEventDispatcher>()
            .As<IDomainEventDispatcher>()
            .InstancePerLifetimeScope();
    }
}
