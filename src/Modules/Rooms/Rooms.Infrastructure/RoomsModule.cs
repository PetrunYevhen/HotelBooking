using Autofac;
using Rooms.Application.Contracts;
using Rooms.Infrastructure.Configuration;
using Rooms.Infrastructure.Configuration.Processing;

namespace Rooms.Infrastructure;

public class RoomsModule : IRoomsModule
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
        using (var scope = RoomsCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<MediatR.IMediator>();
            return mediator.Send(command);
        }
        
    }
}