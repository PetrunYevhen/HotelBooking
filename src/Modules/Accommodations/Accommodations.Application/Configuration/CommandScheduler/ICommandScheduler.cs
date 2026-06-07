using Hotels.Application.Contracts;

namespace Hotels.Application.Configuration.CommandScheduler;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<T>(ICommand<T> command);
}