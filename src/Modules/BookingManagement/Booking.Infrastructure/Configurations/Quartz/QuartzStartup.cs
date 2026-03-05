using System.Collections.Specialized;
using BookingManagement.Infrastructure.Configurations.Processing.Outbox;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace BookingManagement.Infrastructure.Configurations.Quartz;

internal static class QuartzStartup
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Initializing Quartz Scheduler...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "BookingManagement");

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        _scheduler.Start().GetAwaiter().GetResult();
        
        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();

        ITrigger trigger;
        if (internalProcessingPoolingInterval.HasValue)
        {
            trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                            .RepeatForever())
                    .Build();
        }
        else
        {
            trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/2 * * ? * *")
                    .Build();
        }

        _scheduler
            .ScheduleJob(processOutboxJob, trigger)
            .GetAwaiter().GetResult();

    }
    
}