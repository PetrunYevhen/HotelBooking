using Application.Emails;
using Autofac;
using Infrastructure.Emails;

namespace Notifications.Infrastructure.Configuration.Email;

public class EmailModule : Module
{
    private readonly SmtpSettings _settings;

    public EmailModule(SmtpSettings settings)
    {
        _settings = settings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EmailSender>()
            .As<IEmailSender>()
            .WithParameter("host", _settings.Host)
            .WithParameter("port", _settings.Port)
            .WithParameter("from", _settings.From)
            .WithParameter("user", _settings.User)
            .WithParameter("password", _settings.Password)
            .InstancePerLifetimeScope();
    }
}