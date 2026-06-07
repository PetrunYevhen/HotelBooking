using Autofac;
using Dapper;
using Hotels.Infastructure.Configuration.DataAccess;
using Hotels.Infastructure.Configuration.EventBus;
using Hotels.Infastructure.Configuration.Logging;
using Hotels.Infastructure.Configuration.Mediation;
using Hotels.Infastructure.Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Hotels.Infastructure.Configuration;

public class HotelStartup
{
   private static IContainer _container;
   private readonly IConfiguration _configuration;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus)
   {
       SqlMapper.AddTypeHandler(new HotelsIdTypeHandler());
       
       var moduleLogger = logger.ForContext("Module", "HotelManagement");
       
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
       
       _container = containerBuilder.Build();
       
       HotelsCompositoryRoot.SetContainer(_container);
   }
}