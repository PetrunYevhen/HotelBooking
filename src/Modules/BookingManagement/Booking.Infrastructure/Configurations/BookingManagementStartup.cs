using Autofac;
using BookingManagement.Application.Events;
using BookingManagement.Infrastructure.Configurations.Client;
using BookingManagement.Infrastructure.Configurations.DataAccess;
using BookingManagement.Infrastructure.Configurations.EventBus;
using BookingManagement.Infrastructure.Configurations.Logging;
using BookingManagement.Infrastructure.Configurations.Mediation;
using BookingManagement.Infrastructure.Configurations.Processing;
using BookingManagement.Infrastructure.Configurations.Processing.Outbox;
using BookingManagement.Infrastructure.Configurations.Quartz;
using BookingManagement.Infrastructure.Dapper;
using Dapper;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.EventBus;
using Serilog;
using Serilog.Extensions.Logging;

namespace BookingManagement.Infrastructure.Configurations;

public class BookingManagementStartup
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

        var moduleLogger = logger.ForContext("Module", "Facilities");
        
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
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "HotelManagement")));
        var loggerFactory = new SerilogLoggerFactory(logger);
        
        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new ProcessingModule());

        var domainNotificationMap = new BiDictionary<string, Type>();
        domainNotificationMap.Add("BookingCreatedNotification", typeof(BookingCreatedNotification));
        domainNotificationMap.Add("BookingCanceledNotification", typeof(BookingCanceledNotification));
        
        containerBuilder.RegisterModule(new OutboxModule(domainNotificationMap));
        containerBuilder.RegisterModule(new QuartzModule());
        containerBuilder.RegisterModule(new ClientModule(client));
        
        _container = containerBuilder.Build();
        
        BookingCompositoryRoot.SetContainer(_container);
    }
}