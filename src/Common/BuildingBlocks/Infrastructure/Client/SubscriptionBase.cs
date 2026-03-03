using Autofac;
using MediatR;

namespace Infrastructure.Client;

public abstract class SubscriptionBase<TRequest, TResponse, TQuery> : ISubscription 
    where TQuery : IRequest<TResponse>
    where TRequest : class
{
    private readonly string _route;

    public SubscriptionBase(string route)
    {
        _route = route;
    }
    
    public void Subscride(IClient client, ILifetimeScope lifetimeScope)
    {
        client.Subscribe<TRequest, TResponse>(_route, async (request, cancellationToken) =>
        {
            using var scope = lifetimeScope.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            var query = MapToQuery(request);
            return await mediator.Send(query, cancellationToken);
        });
   } 
    protected abstract TQuery MapToQuery(TRequest request);
    
}

