using System.Collections.Specialized;
using Notifications.Infrastructure.Configuration.Processing.Inbox;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Notifications.Infrastructure.Configuration.Quartz;

internal static class QuartzStartup
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Initializing Notifications Quartz Scheduler...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Notifications");

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        _scheduler.Start().GetAwaiter().GetResult();
        
        ITrigger trigger;
        
        var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();
        if (internalProcessingPoolingInterval.HasValue)
        {
            trigger = TriggerBuilder.Create().StartNow()
                .WithSimpleSchedule(x =>
                    x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                        .RepeatForever())
                .Build();
        }
        else
        {
            trigger = TriggerBuilder.Create().StartNow()
                .WithCronSchedule("0/2 * * ? * *")
                .Build();
        }

        _scheduler.ScheduleJob(processInboxJob, trigger).GetAwaiter().GetResult();
    }
}
