using Autofac;
using MediatR;
using Payments.Application.Contracts;
using Payments.Infrastructure.Configuration;
using Payments.Infrastructure.Configuration.Processing;

namespace Payments.Infrastructure;

public class PaymentsModule : IPaymentsModule
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
        using (var scope = PaymentCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}