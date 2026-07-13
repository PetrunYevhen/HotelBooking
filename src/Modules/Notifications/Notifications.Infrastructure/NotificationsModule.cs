using Autofac;
using MediatR;
using Notifications.Application.Contracts;
using Notifications.Infrastructure.Configuration;
using Notifications.Infrastructure.Configuration.Processing;

namespace Notifications.Infrastructure;

public class NotificationsModule : INotificationsModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = NotificationsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return mediator.Send(query);
    }
}
