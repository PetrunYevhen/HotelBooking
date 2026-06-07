using Autofac;
using Hotels.Application.Contracts;
using Hotels.Infastructure.Configuration;
using Hotels.Infastructure.Configuration.Processing;
using MediatR;
using ICommand = Hotels.Application.Contracts.ICommand;

namespace Hotels.Infastructure;

public class HotelsModule : IHotelsModule
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
        using (var scope = HotelsCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}