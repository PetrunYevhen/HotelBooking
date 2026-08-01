using Autofac;
using Users.Application.Services;
using Users.Infrastructure.Services;

namespace Users.Infrastructure.Configuration.Services;

internal class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Argon2idPasswordHashingService>()
            .As<IPasswordHashingService>()
            .SingleInstance();
        builder.RegisterType<RefreshTokenService>()
            .As<IRefreshTokenService>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(Assemblies.Application)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
