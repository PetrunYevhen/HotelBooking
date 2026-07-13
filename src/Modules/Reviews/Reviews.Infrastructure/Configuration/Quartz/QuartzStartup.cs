using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Reviews.Infrastructure.Configuration.Processing.Inbox;
using Reviews.Infrastructure.Configuration.Processing.Outbox;
using Reviews.Infrastructure.Configuration.Processing.Services;
using Serilog;

namespace Reviews.Infrastructure.Configuration.Quartz;

internal static class QuartzStartup
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Initializing Reviews Quartz Scheduler...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Reviews");

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        _scheduler.Start().GetAwaiter().GetResult();

        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();

        ITrigger trigger;
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

        _scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

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

        var recalculateRatingJob = JobBuilder.Create<RecalculateRatingJob>().Build();
        var dailyTrigger = TriggerBuilder.Create().StartNow()
            .WithCronSchedule("0 0 0 * * ?")
            .Build();
        _scheduler.ScheduleJob(recalculateRatingJob, dailyTrigger).GetAwaiter().GetResult();
    }
}
