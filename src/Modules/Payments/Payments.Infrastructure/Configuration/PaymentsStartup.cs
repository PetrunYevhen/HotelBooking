using Autofac;
using Dapper;
using Infrastructure;
using Infrastructure.EventBus;
using Payments.Application.Events;
using Payments.Application.Events.EventNotifications;
using Payments.Infrastructure.Configuration.DataAccess;
using Payments.Infrastructure.Configuration.EventBus;
using Payments.Infrastructure.Configuration.Logging;
using Payments.Infrastructure.Configuration.Mediation;
using Payments.Infrastructure.Configuration.Processing;
using Payments.Infrastructure.Configuration.Processing.Outbox;
using Payments.Infrastructure.Configuration.Quartz;
using Payments.Infrastructure.Dapper;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Payments.Infrastructure.Configuration;

public class PaymentsStartup
{
   private static IContainer _container;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus,
       long? internalProcessingPoolingInterval = null)
   {
       SqlMapper.AddTypeHandler(new PaymentIdTypeHandler());
       
       var moduleLogger = logger.ForContext("Module", "Payments");
       
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
       containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Payments")));
       
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