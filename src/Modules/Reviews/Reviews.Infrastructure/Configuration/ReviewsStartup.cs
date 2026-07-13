using Autofac;
using Infrastructure;
using Infrastructure.EventBus;
using Reviews.Application.Events.EventNotifications;
using Reviews.Infrastructure.Configuration.DataAccess;
using Reviews.Infrastructure.Configuration.EventBus;
using Reviews.Infrastructure.Configuration.Logging;
using Reviews.Infrastructure.Configuration.Mediation;
using Reviews.Infrastructure.Configuration.Processing;
using Reviews.Infrastructure.Configuration.Processing.Outbox;
using Reviews.Infrastructure.Configuration.Quartz;
using Reviews.Infrastructure.Configuration.Services;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Reviews.Infrastructure.Configuration;

public class ReviewsStartup
{
    private static IContainer _container;

    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        long? internalProcessingPoolingInterval = null)
    {
        var moduleLogger = logger.ForContext("Module", "Reviews");

        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus);

        EventBusStartup.Initialize(moduleLogger);
        QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);
    }

    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Reviews")));

        var loggerFactory = new SerilogLoggerFactory(logger);

        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new ServicesModule());
        
        var domainNotificationMap = new BiDictionary<string,Type>();
        domainNotificationMap.Add("HotelRatingUpdatedNotification", typeof(HotelRatingUpdatedNotification));
        domainNotificationMap.Add("ReviewPublishedNotification", typeof(ReviewPublishedNotification));
        domainNotificationMap.Add("ReviewCreatedNotification", typeof(ReviewCreatedNotification));

        containerBuilder.RegisterModule(new OutboxModule(domainNotificationMap));
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new QuartzModule());

        _container = containerBuilder.Build();

        ReviewsCompositionRoot.SetContainer(_container);
    }
}
