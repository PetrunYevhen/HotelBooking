using Autofac;
using Dapper;
using Infrastructure.Client;
using Infrastructure.EventBus;
using RoomManagement.Infrastructure.Configuration.Client;
using RoomManagement.Infrastructure.Configuration.DataAccess;
using RoomManagement.Infrastructure.Configuration.EventBus;
using RoomManagement.Infrastructure.Configuration.Logging;
using RoomManagement.Infrastructure.Configuration.Mapping;
using RoomManagement.Infrastructure.Configuration.Mediation;
using RoomManagement.Infrastructure.Configuration.Quartz;
using RoomManagement.Infrastructure.Dapper;
using Serilog;
using Serilog.Extensions.Logging;
using IContainer = System.ComponentModel.IContainer;

namespace RoomManagement.Infrastructure.Configuration;

public class RoomManagementStartup
{
    private static IContainer _container;
    
    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        IClient client,
        long? internalProcessingPoolingInterval = null)
    {
        SqlMapper.AddTypeHandler(new RoomIdTypeHandler());
        
        var moduleLogger = logger.ForContext("Module", "RoomManagement");
        
        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus, client);
        
        EventBusStartup.Initialize(moduleLogger);
        QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);
    }
    
    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        IClient client)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "RoomManagement")));
        
        var loggerFactory = new SerilogLoggerFactory(logger);

        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new AutoMapperModule());
        containerBuilder.RegisterModule(new ClientModule(client));
        containerBuilder.RegisterModule(new QuartzModule());
        
        var container = containerBuilder.Build();
        
        RoomManagementCompositoryRoot.SetContainer(container);
    }
}