using Autofac;
using RoomManagement.Application.Contracts;
using RoomManagement.Infrastructure.Configuration;
using RoomManagement.Infrastructure.Configuration.Processing;

namespace RoomManagement.Infrastructure;

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