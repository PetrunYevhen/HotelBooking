using Application.Outbox;
using HotelManagement.Domain.Entities;
using HotelManagement.Infastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelManagement.Infastructure;

public class HotelDbContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public HotelDbContext(DbContextOptions<HotelDbContext> options, ILoggerFactory loggerFactory) 
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HotelEntityTypeConfiguration());
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}