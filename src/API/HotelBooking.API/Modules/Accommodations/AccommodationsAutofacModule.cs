using Accommodations.Application.Contracts;
using Accommodations.Infrastructure;
using Autofac;

namespace HotelBooking.API.Modules.Accommodations;

public class AccommodationsAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccommodationsModule>()
            .As<IAccommodationsModule>()
            .InstancePerLifetimeScope();
    }
}