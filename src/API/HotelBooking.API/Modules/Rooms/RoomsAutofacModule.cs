using Autofac;
using Rooms.Application.Contracts;
using Rooms.Infrastructure;

namespace HotelBooking.API.Modules.Rooms;

public class RoomManagementAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RoomsModule>()
            .As<IRoomsModule>()
            .InstancePerLifetimeScope();
    }
}