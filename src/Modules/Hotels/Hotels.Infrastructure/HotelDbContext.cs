using Application.Outbox;
using Hotels.Domain.Entities;
using Hotels.Infastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hotels.Infastructure;

public class HotelDbContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public HotelDbContext(DbContextOptions<HotelDbContext> options, ILoggerFactory loggerFactory) 
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HotelsEntityTypeConfiguration());
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}