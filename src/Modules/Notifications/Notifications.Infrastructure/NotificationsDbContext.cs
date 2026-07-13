using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure;

public class NotificationsDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options, ILoggerFactory loggerFactory)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
