using Accommodations.Application.Contracts;

namespace Accommodations.Application.Configuration.CommandScheduler;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<T>(ICommand<T> command);
}