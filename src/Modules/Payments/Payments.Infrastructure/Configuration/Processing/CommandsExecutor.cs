using Autofac;
using MediatR;
using Payments.Application.Contracts;

namespace Payments.Infrastructure.Configuration.Processing;

public class CommandsExecutor
{
    internal static async Task Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        await using var scope = PaymentCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(command, cancellationToken);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        await using var scope = PaymentCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(command, cancellationToken);
    }
}
