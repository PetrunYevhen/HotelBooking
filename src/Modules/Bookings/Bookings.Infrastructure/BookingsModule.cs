using Autofac;
using Bookings.Application.Contracts;
using Bookings.Infrastructure.Configurations;
using Bookings.Infrastructure.Configurations.Processing;
using MediatR;

namespace Bookings.Infrastructure;

public class BookingsModule : IBookingsModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }
    

    public Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using (var scope = BookingCompositoryRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return mediator.Send(query);
        }
    }
}