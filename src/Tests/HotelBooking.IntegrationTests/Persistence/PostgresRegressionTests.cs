using Accommodations.Domain.Entities.HotelierApplications;
using Accommodations.Infrastructure;
using Bookings.Domain.Entities;
using Bookings.Domain.ValueObjects;
using Bookings.Infrastructure;
using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using SharedKernel.Contracts;
using SharedKernel.ValueObjects;
using Xunit;
using Autofac;
using Infrastructure;
using Infrastructure.Inbox;
using MediatR;
using Newtonsoft.Json;
using Payments.Infrastructure;
using Payments.Application.GatewayContract;
using Payments.Domain.Entities;
using Payments.Domain.Entities.Enums;
using BuildingBlock.Domain;
using Bookings.IntegrationEvents;
using Infrastructure.EventBus;
using Users.Infrastructure;
using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;

namespace HotelBooking.IntegrationTests.Persistence;

public sealed class PostgresFactAttribute : FactAttribute
{
    public PostgresFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("HOTELBOOKING_TEST_CONNECTION")))
            Skip = "Set HOTELBOOKING_TEST_CONNECTION to run isolated PostgreSQL regression tests.";
    }
}

public sealed class PostgresRegressionTests
{
    [PostgresFact]
    public async Task ApprovalCommitsOutboxAndPromotesAccountThroughInbox()
    {
        await using var database = await TestDatabase.CreateAsync();
        var user = User.Create("applicant", "test-password-hash", "applicant@example.com",
            UserPersonalInfo.Create("Test", "Applicant", "+380501234567").Value).Value;
        await using (var users = database.Users())
        {
            users.Add(user);
            await users.SaveChangesAsync();
        }
        var application = HotelierApplication.Create(new AccountId(user.UserId.Value), "Hotel Ltd", "REG-1", null,
            "hotel@example.com", "+380501234567", "Hotel", "Street 1").Value;
        await using (var seed = database.Accommodations())
        {
            seed.Add(application);
            await seed.SaveChangesAsync();
        }
        var builder = new ContainerBuilder();
        builder.RegisterInstance(new Serilog.LoggerConfiguration().CreateLogger()).As<Serilog.ILogger>();
        builder.RegisterModule(new Accommodations.Infrastructure.Configuration.DataAccess.DataAccessModule(database.Connection, NullLoggerFactory.Instance));
        builder.RegisterModule(new Accommodations.Infrastructure.Configuration.Mediation.MediatorModule());
        builder.RegisterModule(new Accommodations.Infrastructure.Configuration.Processing.ProcessingModule());
        var mapping = new BiDictionary<string, Type>();
        mapping.Add("HotelAddOnUpsertedNotification", typeof(Accommodations.Application.Events.EventNotifications.HotelAddOnUpsertedNotification));
        mapping.Add("HotelAddOnDeactivatedNotification", typeof(Accommodations.Application.Events.EventNotifications.HotelAddOnDeactivatedNotification));
        mapping.Add("HotelierApplicationApprovedNotification", typeof(Accommodations.Application.Events.EventNotifications.HotelierApplicationApprovedNotification));
        builder.RegisterModule(new Accommodations.Infrastructure.Configuration.Processing.Outbox.OutboxModule(mapping));
        builder.RegisterInstance(new ApprovalTransport(database)).As<IEventBus>();
        await using var accommodations = builder.Build();
        await using (var scope = accommodations.BeginLifetimeScope())
        {
            var result = await scope.Resolve<IMediator>().Send(
                new Accommodations.Application.HotelierApplications.ReviewHotelierApplicationCommand(application.HotelierApplicationId,
                    new AccountId(Guid.NewGuid()), true, null) { IsAdmin = true });
            Assert.True(result.IsSuccess);
        }
        await using (var persisted = database.Accommodations())
            Assert.Single(await persisted.OutboxMessages.ToListAsync());
        await using (var scope = accommodations.BeginLifetimeScope())
            await scope.Resolve<IMediator>().Send(new Accommodations.Infrastructure.Configuration.Processing.Outbox.ProcessOutboxCommand());

        var iamBuilder = new ContainerBuilder();
        iamBuilder.RegisterInstance(new Serilog.LoggerConfiguration().CreateLogger()).As<Serilog.ILogger>();
        iamBuilder.RegisterModule(new Users.Infrastructure.Configuration.DataAccess.DataAccessModule(database.Connection, NullLoggerFactory.Instance));
        iamBuilder.RegisterModule(new Users.Infrastructure.Configuration.Mediation.MediatorModule());
        iamBuilder.RegisterModule(new Users.Infrastructure.Configuration.Processing.ProcessingModule());
        iamBuilder.RegisterModule(new Users.Infrastructure.Configuration.Processing.Outbox.OutboxModule(new BiDictionary<string, Type>()));
        await using var iam = iamBuilder.Build();
        await using (var scope = iam.BeginLifetimeScope())
            await scope.Resolve<IMediator>().Send(new Users.Infrastructure.Configuration.Processing.Inbox.ProcessInboxCommand());
        await using var resultContext = database.Users();
        Assert.Equal(Role.Hotelier, (await resultContext.Users.SingleAsync()).Role);
        Assert.NotNull((await resultContext.InboxMessages.SingleAsync()).ProcessedDate);
    }

    [PostgresFact]
    public async Task FailedRefundStaysInInboxAndSuccessfulRetryCommitsOnce()
    {
        await using var database = await TestDatabase.CreateAsync();
        var payment = Payment.Create(Guid.NewGuid(), Money.Create(100, "EUR").Value).Value;
        payment.Complete("pi_test");
        var notification = new BookingCanceledIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow,
            payment.BookingId, Guid.NewGuid(), 100, "EUR");
        await using (var context = database.Payments())
        {
            context.Add(payment);
            context.Add(new InboxMessage(notification.OccurredOn, notification.GetType().FullName!, JsonConvert.SerializeObject(notification))
                { Id = notification.Id });
            await context.SaveChangesAsync();
        }
        var gateway = new RefundGateway();
        var builder = new ContainerBuilder();
        builder.RegisterInstance(new Serilog.LoggerConfiguration().CreateLogger()).As<Serilog.ILogger>();
        builder.RegisterModule(new Payments.Infrastructure.Configuration.DataAccess.DataAccessModule(database.Connection, NullLoggerFactory.Instance));
        builder.RegisterModule(new Payments.Infrastructure.Configuration.Mediation.MediatorModule());
        builder.RegisterModule(new Payments.Infrastructure.Configuration.Processing.ProcessingModule());
        builder.RegisterModule(new Payments.Infrastructure.Configuration.Processing.Outbox.OutboxModule(new BiDictionary<string, Type>()));
        builder.RegisterInstance(gateway).As<IPaymentGatewayClient>();
        await using var container = builder.Build();
        async Task Process()
        {
            await using var scope = container.BeginLifetimeScope();
            await scope.Resolve<IMediator>().Send(new Payments.Infrastructure.Configuration.Processing.Inbox.ProcessInboxCommand());
        }
        await Process();
        await using (var failed = database.Payments())
        {
            Assert.Null((await failed.InboxMessages.SingleAsync()).ProcessedDate);
            Assert.Equal(PaymentStatus.Completed, (await failed.Payments.SingleAsync()).Status);
        }
        gateway.Succeeds = true;
        await Process();
        await Process();
        await using var successful = database.Payments();
        Assert.NotNull((await successful.InboxMessages.SingleAsync()).ProcessedDate);
        Assert.Equal(PaymentStatus.Refunded, (await successful.Payments.SingleAsync()).Status);
        Assert.Equal(2, gateway.Attempts);
    }

    [PostgresFact]
    public async Task BookingConcurrencyRejectsExpirationAfterConfirmation()
    {
        await using var database = await TestDatabase.CreateAsync();
        var booking = NewBooking(Guid.NewGuid(), DateTime.UtcNow.Date.AddDays(10));
        await using (var seed = database.Bookings())
        {
            seed.Add(booking);
            await seed.SaveChangesAsync();
        }
        await using var first = database.Bookings();
        await using var second = database.Bookings();
        var confirming = await first.Bookings.SingleAsync(x => x.BookingId == booking.BookingId);
        var expiring = await second.Bookings.SingleAsync(x => x.BookingId == booking.BookingId);
        confirming.AcceptPayment(confirming.TotalPrice);
        expiring.Expire();
        await first.SaveChangesAsync();
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());
    }

    [PostgresFact]
    public async Task ConsecutiveStaysSucceedButOverlappingStayIsRejectedByPostgres()
    {
        await using var database = await TestDatabase.CreateAsync();
        var roomId = Guid.NewGuid();
        var start = DateTime.UtcNow.Date.AddDays(10);
        await using var context = database.Bookings();
        context.AddRange(NewBooking(roomId, start), NewBooking(roomId, start.AddDays(1)));
        await context.SaveChangesAsync();
        context.Add(NewBooking(roomId, start));
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        Assert.Equal(PostgresErrorCodes.ExclusionViolation, Assert.IsType<PostgresException>(error.InnerException).SqlState);
    }

    [PostgresFact]
    public async Task ConcurrentApplicationReviewCannotOverwriteApproval()
    {
        await using var database = await TestDatabase.CreateAsync();
        var application = HotelierApplication.Create(new AccountId(Guid.NewGuid()), "Hotel Ltd", "REG-1", null,
            "hotel@example.com", "+380501234567", "Hotel", "Street 1").Value;
        await using (var seed = database.Accommodations())
        {
            seed.Add(application);
            await seed.SaveChangesAsync();
        }
        await using var first = database.Accommodations();
        await using var second = database.Accommodations();
        var approving = await first.HotelierApplications.SingleAsync();
        var rejecting = await second.HotelierApplications.SingleAsync();
        approving.Approve(new AccountId(Guid.NewGuid()));
        rejecting.Reject(new AccountId(Guid.NewGuid()), "Rejected");
        await first.SaveChangesAsync();
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());
    }

    private static Booking NewBooking(Guid roomId, DateTime start) =>
        Booking.Create(Guid.NewGuid(), roomId, Guid.NewGuid(), Money.Create(100, "EUR").Value,
            DateRange.Create(start, start.AddDays(1)).Value, 1,
            GuestInfo.Create("Test", "Guest", "test@example.com", "+380501234567").Value,
            scheduledCheckOutAt: start.AddDays(1).AddHours(12)).Value;

    private sealed class TestDatabase : IAsyncDisposable
    {
        private readonly string _adminConnection;
        private readonly string _databaseName = "review_" + Guid.NewGuid().ToString("N");
        private readonly string _connection;

        private TestDatabase(string adminConnection)
        {
            _adminConnection = adminConnection;
            _connection = new NpgsqlConnectionStringBuilder(adminConnection) { Database = _databaseName }.ConnectionString;
        }

        public static async Task<TestDatabase> CreateAsync()
        {
            var database = new TestDatabase(Environment.GetEnvironmentVariable("HOTELBOOKING_TEST_CONNECTION")!);
            await using var admin = new NpgsqlConnection(database._adminConnection);
            await admin.OpenAsync();
            await new NpgsqlCommand($"CREATE DATABASE \"{database._databaseName}\"", admin).ExecuteNonQueryAsync();
            try
            {
                await using var bookings = database.Bookings();
                await bookings.Database.MigrateAsync();
                await using var accommodations = database.Accommodations();
                await accommodations.Database.MigrateAsync();
                await using var payments = database.Payments();
                await payments.Database.MigrateAsync();
                await using var users = database.Users();
                await users.Database.MigrateAsync();
                return database;
            }
            catch
            {
                await database.DisposeAsync();
                throw;
            }
        }

        private DbContextOptions<T> Options<T>() where T : DbContext => new DbContextOptionsBuilder<T>()
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(_connection).Options;
        public BookingDbContext Bookings() => new(Options<BookingDbContext>(), NullLoggerFactory.Instance);
        public AccommodationsDbContext Accommodations() => new(Options<AccommodationsDbContext>(), NullLoggerFactory.Instance);
        public PaymentsDbContext Payments() => new(Options<PaymentsDbContext>(), NullLoggerFactory.Instance);
        public UsersDbContext Users() => new(Options<UsersDbContext>(), NullLoggerFactory.Instance);
        public string Connection => _connection;

        public async ValueTask DisposeAsync()
        {
            await using var admin = new NpgsqlConnection(_adminConnection);
            await admin.OpenAsync();
            await new NpgsqlCommand($"DROP DATABASE \"{_databaseName}\" WITH (FORCE)", admin).ExecuteNonQueryAsync();
        }
    }

    private sealed class RefundGateway : IPaymentGatewayClient
    {
        public bool Succeeds { get; set; }
        public int Attempts { get; private set; }
        public Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(Money amount, string key, CancellationToken ct) => throw new NotSupportedException();
        public Task<Result<PaymentIntentResult>> ConfirmPaymentIntentAsync(string id, string method, CancellationToken ct) => throw new NotSupportedException();
        public Task<Result<RefundResult>> RefundPaymentAsync(string id, Money amount, string key, CancellationToken ct)
        {
            Attempts++;
            return Task.FromResult(Succeeds ? Result.Success(new RefundResult("refund_test", "succeeded"))
                : Result.Failure<RefundResult>(new Error("PaymentGateway.Unavailable", "Retry later.")));
        }
    }

    private sealed class ApprovalTransport(TestDatabase database) : IEventBus
    {
        public async Task Publish<T>(T notification, CancellationToken cancellationToken) where T : IntegrationEvent
        {
            Assert.IsType<ContractIntegrationEvent<HotelierApplicationApproved>>(notification);
            await using var users = database.Users();
            users.Add(new InboxMessage(notification.OccurredOn, notification.GetType().FullName!, JsonConvert.SerializeObject(notification))
                { Id = notification.Id });
            await users.SaveChangesAsync(cancellationToken);
        }
        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent { }
        public void StartConsuming() { }
        public void Dispose() { }
    }
}
