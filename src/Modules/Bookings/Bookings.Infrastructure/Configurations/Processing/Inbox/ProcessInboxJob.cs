using Quartz;

namespace Bookings.Infrastructure.Configurations.Processing.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}
