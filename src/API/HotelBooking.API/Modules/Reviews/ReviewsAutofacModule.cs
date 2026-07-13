using Autofac;
using Reviews.Application.Contracts;
using Reviews.Infrastructure;

namespace HotelBooking.API.Modules.Reviews;

public class ReviewsAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ReviewsModule>()
            .As<IReviewsModule>()
            .InstancePerLifetimeScope();
    }
}