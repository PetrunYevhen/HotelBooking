using HotelManagement.Application.Contracts;

namespace HotelManagement.Application.Configuration.CommandScheduler;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<T>(ICommand<T> command);
}