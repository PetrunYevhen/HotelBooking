using Application.Outbox;
using BookingManagement.Domain.Entities;
using BookingManagement.Infrastructure.EntityTypeConfiguration;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingManagement.Infrastructure;

public class BookingDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage>  InboxMessages { get; set; }
    
    public BookingDbContext(DbContextOptions<BookingDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}