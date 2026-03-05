using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomManagement.Domain.Entities;
using RoomManagement.Infrastructure.EntityTypeConfiguration;

namespace RoomManagement.Infrastructure;

public class RoomDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }
    
    public RoomDbContext(DbContextOptions<RoomDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoomEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}