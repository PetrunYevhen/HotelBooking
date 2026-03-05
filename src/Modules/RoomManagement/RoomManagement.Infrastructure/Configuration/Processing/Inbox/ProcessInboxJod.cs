using Quartz;

namespace RoomManagement.Infrastructure.Configuration.Processing.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJod : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}
