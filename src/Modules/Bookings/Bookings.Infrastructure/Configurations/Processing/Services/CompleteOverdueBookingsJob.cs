using Quartz;

namespace Bookings.Infrastructure.Configurations.Processing.Services;


[DisallowConcurrentExecution]
public class CompleteOverdueBookingsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new CompleteOverdueBookingsCommand());
    }
}