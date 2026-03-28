namespace Rooms.Application.Contracts;

public interface IRoomsModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    Task ExecuteCommandAsync(ICommand command);
    
    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}