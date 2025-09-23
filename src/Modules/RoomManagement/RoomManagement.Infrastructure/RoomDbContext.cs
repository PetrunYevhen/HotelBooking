using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomManagment.Domain.Entities;
using RoomManagment.Infrastructure.EntityTypeConfiguration;

namespace RoomManagment.Infrastructure;

public class RoomDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    
    public RoomDbContext(DbContextOptions<RoomDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoomEntityTypeConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}