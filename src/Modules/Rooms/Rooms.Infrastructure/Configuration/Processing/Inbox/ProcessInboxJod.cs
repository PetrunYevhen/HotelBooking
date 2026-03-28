using Quartz;

namespace Rooms.Infrastructure.Configuration.Processing.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJod : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}
