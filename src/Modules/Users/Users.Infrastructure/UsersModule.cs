using Autofac;
using MediatR;
using Users.Application.Contracts;
using Users.Infrastructure.Configuration;
using Users.Infrastructure.Configuration.Processing;

namespace Users.Infrastructure;

public class UsersModule : IUsersModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await CommandsExecutor.Execute(command, cancellationToken);
    }

    public async Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        await CommandsExecutor.Execute(command, cancellationToken);
    }
    

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = UserCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}
