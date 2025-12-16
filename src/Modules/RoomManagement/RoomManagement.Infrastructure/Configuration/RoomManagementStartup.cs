using Autofac;
using Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using RoomManagement.Application.Mapping;
using RoomManagement.Infrastructure.Configuration.DataAccess;
using RoomManagement.Infrastructure.Configuration.EventBus;
using RoomManagement.Infrastructure.Configuration.Logging;
using RoomManagement.Infrastructure.Configuration.Mapping;
using RoomManagement.Infrastructure.Configuration.Mediation;
using RoomManagement.Infrastructure.Dapper;
using Serilog;
using Serilog.Extensions.Logging;
using IContainer = System.ComponentModel.IContainer;

namespace RoomManagement.Infrastructure.Configuration;

public class RoomManagementStartup
{
    private readonly IConfiguration _configuration;
    private readonly IContainer _container;
    
    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        SqlMapper.AddTypeHandler(new RoomIdTypeHandler());
        
        var moduleLogger = logger.ForContext("Module", "RoomManagement");
        
        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus);
        
        EventBusStartup.Initialize(moduleLogger);
    }
    
    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "RoomManagement")));
        
        var loggerFactory = new SerilogLoggerFactory(logger);

        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new AutoMapperModule(typeof(RoomProfile).Assembly));
        
        var container = containerBuilder.Build();
        
        RoomManagementCompositoryRoot.SetContainer(container);
    }
}