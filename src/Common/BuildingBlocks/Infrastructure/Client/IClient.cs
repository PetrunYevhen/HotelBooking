namespace Infrastructure.Client;

public interface IClient: IDisposable
{
    Task SendAsync(string path, object request, CancellationToken cancellationToken);
    Task<TResult> SendAsync<TResult>(string path, object request, CancellationToken cancellationToken);
    Task PublishAsync(object message, CancellationToken cancellationToken);
    IClient Subscribe<TRequest, TResponse>(
        string path,
        Func<TRequest, CancellationToken, Task<TResponse>> handler) where TRequest : class;

}