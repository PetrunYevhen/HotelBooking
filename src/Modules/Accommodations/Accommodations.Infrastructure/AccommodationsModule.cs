using Accommodations.Application.Contracts;
using Accommodations.Infrastructure.Configuration;
using Accommodations.Infrastructure.Configuration.Processing;
using Autofac;
using MediatR;
using ICommand = Accommodations.Application.Contracts.ICommand;

namespace Accommodations.Infrastructure;

public class AccommodationsModule : IAccommodationsModule
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
        using (var scope = AccommodationsCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}