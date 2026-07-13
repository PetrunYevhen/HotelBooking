using Autofac;
using MediatR;
using Reviews.Application.Contracts;

namespace Reviews.Infrastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        await using var scope = ReviewsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(command, cancellationToken);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        await using var scope = ReviewsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(command, cancellationToken);
    }
}
