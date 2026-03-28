using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rooms.Domain.Entities;
using Rooms.Infrastructure.EntityTypeConfiguration;

namespace Rooms.Infrastructure;

public class RoomsDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }
    
    public RoomsDbContext(DbContextOptions<RoomsDbContext> options, ILoggerFactory loggerFactory)
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