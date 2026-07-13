using Accommodations.Application.Contracts;
using Accommodations.Infrastructure.Configuration;
using Accommodations.Infrastructure.Configuration.Processing;
using Autofac;
using MediatR;
using ICommand = Accommodations.Application.Contracts.ICommand;

namespace Accommodations.Infrastructure;

public class AccommodationsModule : IAccommodationsModule
{
    public Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return CommandsExecutor.Execute(command, cancellationToken);
    }

    public Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        return CommandsExecutor.Execute(command, cancellationToken);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        await using var scope = AccommodationsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}
