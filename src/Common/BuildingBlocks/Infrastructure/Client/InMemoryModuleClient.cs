using System.Collections.Concurrent;
using System.Text.Json;
using Autofac;

namespace Infrastructure.Client;

public class InMemoryModuleClient : IClient
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly ConcurrentDictionary<string, Func<object, CancellationToken, Task<object?>>> _routes = new();

    public InMemoryModuleClient(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public void Dispose()
    {
        _routes.Clear(); 
    }

    public async Task SendAsync(string path, object request, CancellationToken cancellationToken)
    {
        if(!_routes.TryGetValue(path, out var handler))
            throw new InvalidOperationException($"No handler registered for {path}");
        
        await handler(request, cancellationToken);
    }

    public async Task<TResult> SendAsync<TResult>(string path, object request, CancellationToken cancellationToken)
    {
        if(!_routes.TryGetValue(path, out var handler))
            throw new InvalidOperationException($"No handler registered for {path}");
        
        var result = await handler(request, cancellationToken);
        return Map<TResult>(result);    
    }

    public Task PublishAsync(object message, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IClient Subscribe<TRequest, TResponse>(
        string path,
        Func<TRequest, CancellationToken, Task<TResponse>> handler) 
        where TRequest : class 
    {
        _routes.TryAdd(path, async (rawRequest, token) =>
        {
            var typedRequest = Map<TRequest>(rawRequest);
            
                return await handler(typedRequest, token);
        });
    
        return this;
    }
    
    private static T Map<T>(object? data)
    {
        if (data is null) return default!;
        if (data is T t) return t;
        var json = JsonSerializer.Serialize(data);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}