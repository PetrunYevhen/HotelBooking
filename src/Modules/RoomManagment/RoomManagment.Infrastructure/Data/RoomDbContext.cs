using Microsoft.EntityFrameworkCore;
using RoomManagment.Domain.Entities;

namespace RoomManagment.Infrastructure.Data;

public class RoomDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    
    public RoomDbContext(DbContextOptions<RoomDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Rooms");

        modelBuilder.Entity<Room>()
            .ToTable("Rooms");

        modelBuilder.Entity<Room>()
            .Property(x => x.RoomId)
            .HasConversion(
                id => id.Value,
                value => new RoomId(value));

        base.OnModelCreating(modelBuilder);
    }
}