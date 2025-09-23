using Autofac;
using BookingManagement.Application.Contracts;
using BookingManagement.Infrastructure.Configurations;
using BookingManagement.Infrastructure.Configurations.Processing;
using MediatR;

namespace BookingManagement.Infrastructure;

public class BookingManagementModule : IBookingManagementModule
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