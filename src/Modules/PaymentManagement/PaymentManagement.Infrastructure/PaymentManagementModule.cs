using Autofac;
using MediatR;
using PaymantManagement.Application.Contracts;
using PaymantManagement.Infrastructure.Configuration;
using PaymantManagement.Infrastructure.Configuration.Processing;

namespace PaymantManagement.Infrastructure;

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
        using (var scope = HotelCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}