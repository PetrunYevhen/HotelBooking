using Autofac;
using Payments.Application.Contracts;
using Payments.Infrastructure;

namespace HotelBooking.API.Modules.Payments;

public class PaymentManagementAutofacModule :  Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PaymentsModule>()
            .As<IPaymentsModule>()
            .InstancePerLifetimeScope();
    }
}