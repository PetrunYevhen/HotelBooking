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

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Migration failed: required environment variable 'ConnectionStrings__DefaultConnection' is not set.");
    return 1;
}

try
{
    await using var accommodations = CreateContext<AccommodationsDbContext>(connectionString);
    await MigrateAsync(accommodations);

    await using var bookings = CreateContext<BookingDbContext>(connectionString);
    await MigrateAsync(bookings);

    await using var payments = CreateContext<PaymentsDbContext>(connectionString);
    await MigrateAsync(payments);

    await using var reviews = CreateContext<ReviewsDbContext>(connectionString);
    await MigrateAsync(reviews);

    await using var notifications = CreateContext<NotificationsDbContext>(connectionString);
    await MigrateAsync(notifications);

    await using var users = CreateContext<UsersDbContext>(connectionString);
    await MigrateAsync(users);

    Console.WriteLine("All database migrations completed successfully.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Migration failed: {exception}");
    return 1;
}

static TContext CreateContext<TContext>(string connectionString)
    where TContext : DbContext
{
    var options = new DbContextOptionsBuilder<TContext>()
        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
        .UseNpgsql(connectionString)
        .Options;

    return (TContext)Activator.CreateInstance(typeof(TContext), options, NullLoggerFactory.Instance)!;
}

static async Task MigrateAsync(DbContext context)
{
    var pendingMigrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

    Console.WriteLine(
        pendingMigrations.Count == 0
            ? $"{context.GetType().Name}: no pending migrations."
            : $"{context.GetType().Name}: applying {pendingMigrations.Count} pending migration(s)."
    );

    await context.Database.MigrateAsync();
    Console.WriteLine($"{context.GetType().Name}: migration completed.");
}
