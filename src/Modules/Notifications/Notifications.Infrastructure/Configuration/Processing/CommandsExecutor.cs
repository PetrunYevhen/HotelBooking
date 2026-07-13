using Autofac;
using MediatR;
using Notifications.Application.Contracts;

namespace Notifications.Infrastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        await using var scope = NotificationsCompositionRoot.BeginLifetimeScope();
        try
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = NotificationsCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
