using Autofac;
using Facilities.Application.Contracts;
using Facilities.Infrastructure.Configurations;
using Facilities.Infrastructure.Configurations.Processing;
using MediatR;

namespace Facilities.Infrastructure;

public class FacilityModule : IFacilityModule
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
        using (var scope = FacilitiesCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}