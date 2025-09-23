namespace Facilities.Application.Contracts;

public interface IFacilityModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    Task ExecuteCommandAsync(ICommand command);
    
    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}