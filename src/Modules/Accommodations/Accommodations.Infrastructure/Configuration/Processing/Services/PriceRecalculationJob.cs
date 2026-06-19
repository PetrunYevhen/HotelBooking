using Quartz;

namespace Accommodations.Infrastructure.Configuration.Processing.Services;

[DisallowConcurrentExecution]
public class PriceRecalculationJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new PriceRecalculationCommand());
    }
}