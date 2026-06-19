using Autofac;
using Bookings.Application.Events.EventNotifications;
using Bookings.Infrastructure.Configurations.Client;
using Bookings.Infrastructure.Configurations.DataAccess;
using Bookings.Infrastructure.Configurations.EventBus;
using Bookings.Infrastructure.Configurations.Logging;
using Bookings.Infrastructure.Configurations.Mediation;
using Bookings.Infrastructure.Configurations.Processing;
using Bookings.Infrastructure.Configurations.Processing.Outbox;
using Bookings.Infrastructure.Configurations.Quartz;
using Bookings.Infrastructure.Dapper;
using Dapper;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.EventBus;
using Serilog;
using Serilog.Extensions.Logging;

namespace Bookings.Infrastructure.Configurations;

public class BookingsStartup
{
    private static IContainer _container;

    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        IClient client,
        long? internalProcessingPoolingInterval = null)
    {
        SqlMapper.AddTypeHandler(new BookingIdTypeHandler());

        var moduleLogger = logger.ForContext("Module", "Bookings");
        
        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus, client);
        
        EventBusStartup.Initialize(moduleLogger);
        QuartzStartup.Initialize(moduleLogger,  internalProcessingPoolingInterval);
    }
    
    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        IClient client)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Bookings")));
        var loggerFactory = new SerilogLoggerFactory(logger);
        
        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new ProcessingModule());

        var domainNotificationMap = new BiDictionary<string, Type>();
        domainNotificationMap.Add("BookingCreatedNotification", typeof(BookingCreatedNotification));
        domainNotificationMap.Add("BookingCanceledNotification", typeof(BookingCanceledNotification));
        domainNotificationMap.Add("BookingConfirmedNotification", typeof(BookingConfirmedNotification));
        
        containerBuilder.RegisterModule(new OutboxModule(domainNotificationMap));
        containerBuilder.RegisterModule(new QuartzModule());
        containerBuilder.RegisterModule(new ClientModule(client));
        
        _container = containerBuilder.Build();
        
        BookingCompositoryRoot.SetContainer(_container);
    }
}