using Autofac;
using Dapper;
using HotelManagement.Infastructure.Configuration.DataAccess;
using HotelManagement.Infastructure.Configuration.EventBus;
using HotelManagement.Infastructure.Configuration.Logging;
using HotelManagement.Infastructure.Configuration.Mediation;
using HotelManagement.Infastructure.Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace HotelManagement.Infastructure.Configuration;

public class HotelStartup
{
   private static IContainer _container;
   private readonly IConfiguration _configuration;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus)
   {
       SqlMapper.AddTypeHandler(new HotelIdTypeHandler());
       
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
       
       HotelCompositoryRoot.SetContainer(_container);
   }
}