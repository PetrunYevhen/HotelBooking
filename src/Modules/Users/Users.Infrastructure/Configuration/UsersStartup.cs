using Autofac;
using Dapper;
using Infrastructure;
using Infrastructure.EventBus;
using Serilog.Extensions.Logging;
using Users.Infrastructure.Configuration.DataAccess;
using Users.Infrastructure.Configuration.EventBus;
using Users.Infrastructure.Configuration.Logging;
using Users.Infrastructure.Configuration.Mediation;
using Users.Infrastructure.Configuration.Processing;
using Users.Infrastructure.Configuration.Processing.Outbox;
using Users.Infrastructure.Configuration.Quartz;
using Users.Infrastructure.Dapper;
using ILogger = Serilog.ILogger;

namespace Users.Infrastructure.Configuration;

public class UsersStartup
{
   private static IContainer _container;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus,
       long? internalProcessingPoolingInterval = null)
   {
       SqlMapper.AddTypeHandler(new UserIdTypeHandler());
       
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
       
       containerBuilder.RegisterModule(new OutboxModule(domainNotificationsMap));
       
       _container = containerBuilder.Build();
       
       UserCompositoryRoot.SetContainer(_container);
   }
}