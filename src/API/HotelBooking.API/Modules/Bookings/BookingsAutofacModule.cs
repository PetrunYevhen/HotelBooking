using Autofac;
using Bookings.Application.Contracts;
using Bookings.Infrastructure;

namespace HotelBooking.API.Modules.Bookings;

public class BookingsAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BookingsModule>()
            .As<IBookingsModule>()
            .InstancePerLifetimeScope();
    }
}