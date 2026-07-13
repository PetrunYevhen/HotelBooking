using Autofac;
using MediatR;
using Reviews.Application.Contracts;
using Reviews.Infrastructure.Configuration;
using Reviews.Infrastructure.Configuration.Processing;

namespace Reviews.Infrastructure;

public class ReviewsModule : IReviewsModule
{
    public Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return CommandsExecutor.Execute(command, cancellationToken);
    }

    public Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        return CommandsExecutor.Execute(command, cancellationToken);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        await using var scope = ReviewsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}
