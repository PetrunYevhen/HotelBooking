using Accommodations.Infrastructure;
using Bookings.Infrastructure;
using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Abstractions;
using Notifications.Infrastructure;
using Payments.Infrastructure;
using Reviews.Infrastructure;
using Users.Infrastructure;
using Xunit;

namespace HotelBooking.IntegrationTests.Persistence;

public sealed class EfCoreMappingTests
{
    [Fact]
    public void EveryModuleDbContext_CanBuildItsRelationalModel()
    {
        using var accommodations = Create<AccommodationsDbContext>();
        using var bookings = Create<BookingDbContext>();
        using var payments = Create<PaymentsDbContext>();
        using var users = Create<UsersDbContext>();
        using var reviews = Create<ReviewsDbContext>();
        using var notifications = Create<NotificationsDbContext>();

        Assert.NotEmpty(accommodations.Model.GetEntityTypes());
        Assert.NotEmpty(bookings.Model.GetEntityTypes());
        Assert.NotEmpty(payments.Model.GetEntityTypes());
        Assert.NotEmpty(users.Model.GetEntityTypes());
        Assert.NotEmpty(reviews.Model.GetEntityTypes());
        Assert.NotEmpty(notifications.Model.GetEntityTypes());
    }

    private static TContext Create<TContext>() where TContext : DbContext
    {
        var options = new DbContextOptionsBuilder<TContext>()
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql("Host=localhost;Database=model_validation;Username=test;Password=test")
            .Options;

        return (TContext)Activator.CreateInstance(
            typeof(TContext), options, NullLoggerFactory.Instance)!;
    }
}
