using System.Collections.Specialized;
using Accommodations.Infrastructure.Configuration.Processing.Inbox;
using Accommodations.Infrastructure.Configuration.Processing.Outbox;
using Accommodations.Infrastructure.Configuration.Processing.Services;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Accommodations.Infrastructure.Configuration.Quartz;

internal static class QuartzStartup
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Initializing Accommodations Quartz Scheduler...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Accommodations");

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        _scheduler.Start().GetAwaiter().GetResult();

        
        //Outbox
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


        //Inbox
        var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();
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
            .ScheduleJob(processInboxJob, trigger)
            .GetAwaiter().GetResult();
        

        // Services
        var priceRecalcJob = JobBuilder.Create<PriceRecalculationJob>().Build();
        var dailyTrigger = TriggerBuilder.Create()
            .StartNow()
            .WithCronSchedule("0 0/1 * * * ?") 
            .Build();
        _scheduler.ScheduleJob(priceRecalcJob, dailyTrigger).GetAwaiter().GetResult();
    }
    
}