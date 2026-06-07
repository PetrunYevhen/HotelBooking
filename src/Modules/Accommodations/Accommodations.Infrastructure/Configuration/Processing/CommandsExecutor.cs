using Autofac;
using Hotels.Application.Contracts;
using MediatR;

namespace Hotels.Infastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using (var scope = HotelsCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }
    
    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = HotelsCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}