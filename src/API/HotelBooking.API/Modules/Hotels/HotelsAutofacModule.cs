using Autofac;
using Hotels.Application.Contracts;
using Hotels.Infastructure;

namespace HotelBooking.API.Modules.Hotels;

public class HotelsAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HotelsModule>()
            .As<IHotelsModule>()
            .InstancePerLifetimeScope();
    }
}