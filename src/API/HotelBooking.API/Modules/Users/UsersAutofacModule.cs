using Autofac;
using Users.Application.Contracts;
using Users.Infrastructure;

namespace HotelBooking.API.Modules.Users;

public sealed class UsersAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UsersModule>()
            .As<IUsersModule>()
            .InstancePerLifetimeScope();
    }
}
