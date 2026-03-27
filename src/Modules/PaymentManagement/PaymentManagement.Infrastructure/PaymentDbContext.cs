using Application.Outbox;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentManagement.Domain.Entities;
using PaymentManagement.Infrastructure.EntityTypeConfiguration;

namespace PaymentManagement.Infrastructure;

public class PaymentDbContext : DbContext
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options, ILoggerFactory loggerFactory):  base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    
}