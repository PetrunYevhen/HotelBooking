using Quartz;

namespace Bookings.Infrastructure.Configurations.Processing.Services.ExpirePendingBooking;

[DisallowConcurrentExecution]
public class ExpirePendingBookingsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ExpirePendingBookingsCommand());
    }
}
