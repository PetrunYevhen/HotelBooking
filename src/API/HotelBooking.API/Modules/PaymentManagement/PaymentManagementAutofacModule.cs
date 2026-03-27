using Autofac;
using PaymentManagement.Application.Contracts;
using PaymentManagement.Infrastructure;

namespace HotelBooking.API.Modules.PaymentManagement;

public class PaymentManagementAutofacModule :  Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PaymentManagementModule>()
            .As<IPaymentManagementModule>()
            .InstancePerLifetimeScope();
    }
}