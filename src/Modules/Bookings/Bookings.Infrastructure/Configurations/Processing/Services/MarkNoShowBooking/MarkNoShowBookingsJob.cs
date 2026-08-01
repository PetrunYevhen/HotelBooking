using Quartz;

namespace Bookings.Infrastructure.Configurations.Processing.Services.MarkNoShowBooking;

[DisallowConcurrentExecution]
public class MarkNoShowBookingsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new MarkNoShowBookingsCommand());
    }
}
