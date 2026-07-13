using System.Collections.Specialized;
using Bookings.Infrastructure.Configurations.Processing.Inbox;
using Bookings.Infrastructure.Configurations.Processing.Outbox;
using Bookings.Infrastructure.Configurations.Processing.Services;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Bookings.Infrastructure.Configurations.Quartz;

internal static class QuartzStartup
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Initializing Quartz Scheduler...");

        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Bookings");

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


        //CompleteOverdueBookings
        var completeOverdueBookingsJob = JobBuilder.Create<CompleteOverdueBookingsJob>().Build();
        var completeOverdueBookingsTrigger =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithSimpleSchedule(s => s.WithIntervalInMinutes(30).RepeatForever())
                .Build();

        _scheduler
            .ScheduleJob(completeOverdueBookingsJob, completeOverdueBookingsTrigger)
            .GetAwaiter().GetResult();
    }
}