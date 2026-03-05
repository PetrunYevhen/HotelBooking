using Autofac;
using HotelManagement.Application.Contracts;
using HotelManagement.Infastructure.Configuration;
using HotelManagement.Infastructure.Configuration.Processing;
using MediatR;

namespace HotelManagement.Infastructure;

public class HotelManagementModule : IHotelManagementModule
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