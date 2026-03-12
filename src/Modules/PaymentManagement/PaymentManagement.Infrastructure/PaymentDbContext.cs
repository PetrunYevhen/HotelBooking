using Application.Outbox;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentManagement.Domain.Entities;

namespace PaymantManagement.Infrastructure;

public class PaymentDbContext : DbContext
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options, ILoggerFactory loggerFactory):  base(options){}
}