using Autofac;
using Dapper;
using Infrastructure;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using PaymentManagement.Application.Events;
using PaymentManagement.Infrastructure.Configuration.DataAccess;
using PaymentManagement.Infrastructure.Configuration.EventBus;
using PaymentManagement.Infrastructure.Configuration.Logging;
using PaymentManagement.Infrastructure.Configuration.Mediation;
using PaymentManagement.Infrastructure.Configuration.Processing;
using PaymentManagement.Infrastructure.Configuration.Processing.Outbox;
using PaymentManagement.Infrastructure.Configuration.Quartz;
using PaymentManagement.Infrastructure.Dapper;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace PaymentManagement.Infrastructure.Configuration;

public class PaymentStartup
{
   private static IContainer _container;
   private readonly IConfiguration _configuration;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus,
       long? internalProcessingPoolingInterval = null)
   {
       SqlMapper.AddTypeHandler(new PaymentIdTypeHandler());
       
       var moduleLogger = logger.ForContext("Module", "HotelManagement");
       
       ConfigureCompositionRoot(connectionString, moduleLogger, eventBus);
       
       EventBusStartup.Initialize(moduleLogger);
       QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);
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
       containerBuilder.RegisterModule(new ProcessingModule());
       containerBuilder.RegisterModule(new QuartzModule());
       
       var domainNotificationsMap = new BiDictionary<string, Type>();
       domainNotificationsMap.Add("PaymentCreatedNotification", typeof(PaymentCreatedNotification));
       domainNotificationsMap.Add("PaymentCompletedNotification", typeof(PaymentCompletedNotification));
       
       containerBuilder.RegisterModule(new OutboxModule(domainNotificationsMap));
       
       _container = containerBuilder.Build();
       
       PaymentCompositoryRoot.SetContainer(_container);
   }
}