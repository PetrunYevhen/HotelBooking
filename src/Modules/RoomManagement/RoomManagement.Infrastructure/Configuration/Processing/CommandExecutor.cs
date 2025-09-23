using Autofac;
using MediatR;
using RoomManagment.Application.Contracts;

namespace RoomManagment.Infrastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using (var scope = RoomManagementCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = RoomManagementCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}