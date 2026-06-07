using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Pricing;
using Accommodations.Domain.Entities.Rooms;
using Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accommodations.Infrastructure;

public class AccommodationsDbContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Pricing> Pricing { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public AccommodationsDbContext(DbContextOptions<AccommodationsDbContext> options, ILoggerFactory loggerFactory)
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}