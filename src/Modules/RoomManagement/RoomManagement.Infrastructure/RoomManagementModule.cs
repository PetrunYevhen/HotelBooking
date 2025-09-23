using Autofac;
using RoomManagment.Application.Contracts;
using RoomManagment.Infrastructure.Configuration;
using RoomManagment.Infrastructure.Configuration.Processing;

namespace RoomManagment.Infrastructure;

public class RoomManagementModule : IRoomManagementModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }
    
    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> command)
    {
        using (var scope = RoomManagementCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<MediatR.IMediator>();
            return mediator.Send(command);
        }
        
    }
}