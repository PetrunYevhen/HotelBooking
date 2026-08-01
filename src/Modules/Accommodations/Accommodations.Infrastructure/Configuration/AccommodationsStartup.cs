using Accommodations.Infrastructure.Configuration.Client;
using Accommodations.Infrastructure.Configuration.DataAccess;
using Accommodations.Infrastructure.Configuration.EventBus;
using Accommodations.Infrastructure.Configuration.Logging;
using Accommodations.Infrastructure.Configuration.Mediation;
using Accommodations.Infrastructure.Configuration.Processing;
using Accommodations.Infrastructure.Configuration.Processing.Outbox;
using Accommodations.Infrastructure.Configuration.Quartz;
using Accommodations.Infrastructure.Configuration.Services;
using Accommodations.Infrastructure.Dapper;
using Accommodations.Application.Events.EventNotifications;
using Autofac;
using Dapper;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.EventBus;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Accommodations.Infrastructure.Configuration;

public class AccommodationsStartup
{
   private static IContainer _container;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus,
       IClient client,
       long? internalProcessingPoolingInterval = null)
   {
       SqlMapper.AddTypeHandler(new HotelIdTypeHandler());
       SqlMapper.AddTypeHandler(new HotelAddOnIdTypeHandler());
       
       var moduleLogger = logger.ForContext("Module", "Accommodations");
       
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
       containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Accommodations")));
       
       var loggerFactory = new SerilogLoggerFactory(logger);

       containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
       containerBuilder.RegisterModule(new MediatorModule());
       containerBuilder.RegisterModule(new ProcessingModule());
       containerBuilder.RegisterModule(new ServicesModule());
       var domainNotificationMap = new BiDictionary<string, Type>();
       domainNotificationMap.Add("HotelAddOnUpsertedNotification", typeof(HotelAddOnUpsertedNotification));
       domainNotificationMap.Add("HotelAddOnDeactivatedNotification", typeof(HotelAddOnDeactivatedNotification));
       containerBuilder.RegisterModule(new OutboxModule(domainNotificationMap));
       containerBuilder.RegisterModule(new EventBusModule(eventBus));
       containerBuilder.RegisterModule(new QuartzModule());
       
       var clientModule = new ClientModule(client);
       
       containerBuilder.RegisterModule(clientModule);

       _container = containerBuilder.Build();

       clientModule.RegisterSubscriptions(_container);

       AccommodationsCompositionRoot.SetContainer(_container);
   }
}
