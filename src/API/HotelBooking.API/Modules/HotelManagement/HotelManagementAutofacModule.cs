using Autofac;
using HotelManagement.Application.Contracts;
using HotelManagement.Infastructure;

namespace HotelBooking.API.Modules.HotelManagement;

public class HotelManagementAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HotelManagementModule>()
            .As<IHotelManagementModule>()
            .InstancePerLifetimeScope();
    }
}