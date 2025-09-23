using Autofac;
using RoomManagment.Application.Contracts;
using RoomManagment.Infrastructure;

namespace HotelBooking.API.Modules.RoomManagement;

public class RoomManagementAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RoomManagementModule>()
            .As<IRoomManagementModule>()
            .InstancePerLifetimeScope();
    }
}