using Autofac;
using Bookings.Application.Contracts;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing;

public class CommandsExecutor
{
    internal static async Task Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        await using var scope = BookingCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(command, cancellationToken);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        await using var scope = BookingCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(command, cancellationToken);
    }
}
