using Autofac;
using Facilities.Application.Contracts;
using MediatR;

namespace Facilities.Infrastructure.Configurations.Processing;

public class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using (var scope = FacilitiesCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = FacilitiesCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}