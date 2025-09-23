using Autofac;
using Dapper;
using Facilities.Infrastructure.Configurations.DataAccess;
using Facilities.Infrastructure.Configurations.EventBus;
using Facilities.Infrastructure.Configurations.Logging;
using Facilities.Infrastructure.Configurations.Mediation;
using Facilities.Infrastructure.Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

namespace Facilities.Infrastructure.Configurations;

public class FacilitiesStartup
{
    private readonly IConfiguration _configuration;
    private readonly IContainer _container;

    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        SqlMapper.AddTypeHandler(new FacilityIdTypeHandler());

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
        
        FacilitiesCompositoryRoot.SetContainer(container);
    }
}