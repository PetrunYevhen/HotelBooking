using Autofac;
using Facilities.Application.Contracts;

namespace HotelBooking.API.Modules.Facilities;

public class FacilitiesAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<global::Facilities.Infrastructure.FacilityModule>()
            .As<IFacilityModule>()
            .InstancePerLifetimeScope();
}
}