using BookingManagement.Infrastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingManagement.Infrastructure;

public class BookingDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    
    public DbSet<Domain.Entities.Booking> Reservations { get; set; }
    
    public BookingDbContext(DbContextOptions<BookingDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}