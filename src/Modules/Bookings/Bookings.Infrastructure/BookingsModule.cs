using Autofac;
using Bookings.Application.Contracts;
using Bookings.Infrastructure.Configurations;
using Bookings.Infrastructure.Configurations.Processing;
using MediatR;

namespace Bookings.Infrastructure;

public class BookingsModule : IBookingsModule
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
        await using var scope = BookingCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}
