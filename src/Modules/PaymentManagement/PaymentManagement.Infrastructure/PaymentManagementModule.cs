using Autofac;
using MediatR;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Infrastructure.Configuration;
using PaymentManagement.Infrastructure.Configuration.Processing;
using ICommand = PaymentManagement.Application.Contracts.ICommand;

namespace PaymentManagement.Infrastructure;

public class PaymentManagementModule : IPaymentManagementModule
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