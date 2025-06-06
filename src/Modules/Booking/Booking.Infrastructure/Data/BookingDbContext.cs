using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Data;

public class BookingDbContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; }
    
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Bookings");
        
        modelBuilder.Entity<Reservation>()
            .ToTable("Bookings");

        modelBuilder.Entity<Reservation>(builder =>
        {
            builder
                .Property(x => x.ReservationId)
                .HasConversion(
                    id => id.Value,
                    value => new ReservationId(value));
        });
        
        base.OnModelCreating(modelBuilder);
    }
}