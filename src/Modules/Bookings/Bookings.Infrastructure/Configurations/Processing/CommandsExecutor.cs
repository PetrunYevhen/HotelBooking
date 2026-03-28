using Autofac;
using Bookings.Application.Contracts;
using MediatR;

namespace Bookings.Infrastructure.Configurations.Processing;

public class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using (var scope = BookingCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = BookingCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}