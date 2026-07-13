using Autofac;
using Infrastructure.Emails;
using Infrastructure.EventBus;
using Notifications.Infrastructure.Configuration.DataAccess;
using Notifications.Infrastructure.Configuration.Email;
using Notifications.Infrastructure.Configuration.EventBus;
using Notifications.Infrastructure.Configuration.Logging;
using Notifications.Infrastructure.Configuration.Mediation;
using Notifications.Infrastructure.Configuration.Processing;
using Notifications.Infrastructure.Configuration.Quartz;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Notifications.Infrastructure.Configuration;

public class NotificationsStartup
{
    private static IContainer _container;

    public static void Initialize(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        SmtpSettings smtpSettings,
        long? internalProcessingPoolingInterval = null)
    {
        var moduleLogger = logger.ForContext("Module", "Notifications");

        ConfigureCompositionRoot(connectionString, moduleLogger, eventBus, smtpSettings);

        EventBusStartup.Initialize(moduleLogger);
        QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);
    }

    private static void ConfigureCompositionRoot(
        string connectionString,
        ILogger logger,
        IEventBus eventBus,
        SmtpSettings smtpSettings)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Notifications")));

        var loggerFactory = new SerilogLoggerFactory(logger);

        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new QuartzModule());
        containerBuilder.RegisterModule(new EmailModule(smtpSettings));

        _container = containerBuilder.Build();

        NotificationsCompositionRoot.SetContainer(_container);
    }
}
