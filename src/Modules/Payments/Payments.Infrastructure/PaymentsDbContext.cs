using Application.Outbox;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payments.Domain.Entities;
using Payments.Infrastructure.EntityTypeConfiguration;

namespace Payments.Infrastructure;

public class PaymentsDbContext : DbContext
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options, ILoggerFactory loggerFactory):  base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaymentsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    
}