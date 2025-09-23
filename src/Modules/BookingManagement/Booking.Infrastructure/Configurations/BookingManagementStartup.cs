using Autofac;
using BookingManagement.Infrastructure.Configurations.DataAccess;
using BookingManagement.Infrastructure.Configurations.EventBus;
using BookingManagement.Infrastructure.Configurations.Logging;
using BookingManagement.Infrastructure.Configurations.Mediation;
using BookingManagement.Infrastructure.Dapper;
using Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

namespace BookingManagement.Infrastructure.Configurations;

public class BookingManagementStartup
{
    private readonly IConfiguration _configuration;
    private readonly IContainer _container;

    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        SqlMapper.AddTypeHandler(new BookingIdTypeHandler());

        var moduleLogger = logger.ForContext("Module", "Facilities");
        
        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus);
        
        EventBusStartup.Initialize(moduleLogger);
    }
    
    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "HotelManagement")));
        var loggerFactory = new SerilogLoggerFactory(logger);
        
        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        
        var container = containerBuilder.Build();
        
        BookingCompositoryRoot.SetContainer(container);
    }
}