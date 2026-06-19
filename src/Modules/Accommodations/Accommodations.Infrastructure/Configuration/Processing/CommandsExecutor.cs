using Accommodations.Application.Contracts;
using Autofac;
using MediatR;

namespace Accommodations.Infrastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        await using var scope = AccommodationsCompositionRoot.BeginLifetimeScope();
        try
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        await using var scope = AccommodationsCompositionRoot.BeginLifetimeScope();
        try
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}