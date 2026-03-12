using Autofac;
using Dapper;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using PaymantManagement.Infrastructure.Configuration.Dapper;
using PaymantManagement.Infrastructure.Configuration.DataAccess;
using PaymantManagement.Infrastructure.Configuration.EventBus;
using PaymantManagement.Infrastructure.Configuration.Logging;
using PaymantManagement.Infrastructure.Configuration.Mediation;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace PaymantManagement.Infrastructure.Configuration;

public class PaymentStartup
{
   private static IContainer _container;
   private readonly IConfiguration _configuration;
   
   public static void Initialize(
       string connectionString,
       ILogger logger,
       IEventBus eventBus)
   {
       SqlMapper.AddTypeHandler(new PaymentIdTypeHandler());
       
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
       
       PaymentCompositoryRoot.SetContainer(_container);
   }
}