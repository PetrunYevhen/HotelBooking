using Autofac;
using MediatR;
using Users.Application.Contracts;

namespace Users.Infrastructure.Configuration.Processing;

public class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using (var scope = UserCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using (var scope = UserCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}