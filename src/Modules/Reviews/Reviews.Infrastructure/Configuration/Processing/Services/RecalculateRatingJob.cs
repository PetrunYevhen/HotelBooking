using Quartz;

namespace Reviews.Infrastructure.Configuration.Processing.Services;

[DisallowConcurrentExecution]
public class RecalculateRatingJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new RecalculateRatingCommand());
    }
}