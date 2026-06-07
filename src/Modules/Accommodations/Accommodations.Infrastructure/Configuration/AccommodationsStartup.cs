using Accommodations.Infrastructure.Configuration.DataAccess;
using Accommodations.Infrastructure.Configuration.EventBus;
using Accommodations.Infrastructure.Configuration.Logging;
using Accommodations.Infrastructure.Configuration.Mediation;
using Accommodations.Infrastructure.Configuration.Processing;
using Accommodations.Infrastructure.Configuration.Processing.Outbox;
using Accommodations.Infrastructure.Dapper;
using Autofac;
using Dapper;
using Infrastructure;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Accommodations.Infrastructure.Configuration;

public class AccommodationsStartup
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
       containerBuilder.RegisterModule(new ProcessingModule());
       containerBuilder.RegisterModule(new OutboxModule(new BiDictionary<string, Type>()));
       containerBuilder.RegisterModule(new EventBusModule(eventBus));
       
       _container = containerBuilder.Build();
       
       AccommodationsCompositionRoot.SetContainer(_container);
   }
}