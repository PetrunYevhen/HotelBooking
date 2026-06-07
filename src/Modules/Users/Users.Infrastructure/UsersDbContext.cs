using Application.Outbox;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.Domain.Entities;
using Users.Infrastructure.EntityTypeConfiguration;

namespace Users.Infrastructure;

public class UsersDbContext : DbContext
{
    public DbSet<User> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public UsersDbContext(DbContextOptions<UsersDbContext> options, ILoggerFactory loggerFactory):  base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    
}