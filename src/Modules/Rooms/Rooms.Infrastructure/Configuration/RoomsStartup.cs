using Autofac;
using Dapper;
using Infrastructure.Client;
using Infrastructure.EventBus;
using Rooms.Infrastructure.Configuration.Client;
using Rooms.Infrastructure.Configuration.DataAccess;
using Rooms.Infrastructure.Configuration.EventBus;
using Rooms.Infrastructure.Configuration.Logging;
using Rooms.Infrastructure.Configuration.Mapping;
using Rooms.Infrastructure.Configuration.Mediation;
using Rooms.Infrastructure.Configuration.Quartz;
using Rooms.Infrastructure.Dapper;
using Serilog;
using Serilog.Extensions.Logging;
using IContainer = System.ComponentModel.IContainer;

namespace Rooms.Infrastructure.Configuration;

public class RoomsStartup
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
        
        RoomsCompositoryRoot.SetContainer(container);
    }
}