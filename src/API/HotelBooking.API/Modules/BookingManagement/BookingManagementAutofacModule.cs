using Autofac;
using BookingManagement.Application.Contracts;
using BookingManagement.Infrastructure;

namespace HotelBooking.API.Modules.BookingManagement;

public class BookingManagementAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BookingManagementModule>()
            .As<IBookingManagementModule>()
            .InstancePerLifetimeScope();
    }
}