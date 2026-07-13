using Autofac;
using MediatR;
using Payments.Application.Contracts;
using Payments.Infrastructure.Configuration;
using Payments.Infrastructure.Configuration.Processing;

namespace Payments.Infrastructure;

public class PaymentsModule : IPaymentsModule
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
        await using var scope = PaymentCompositoryRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}
