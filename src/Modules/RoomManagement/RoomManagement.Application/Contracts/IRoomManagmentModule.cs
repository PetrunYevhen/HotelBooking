namespace RoomManagment.Application.Contracts;

public interface IRoomManagementModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    Task ExecuteCommandAsync(ICommand command);
    
    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}