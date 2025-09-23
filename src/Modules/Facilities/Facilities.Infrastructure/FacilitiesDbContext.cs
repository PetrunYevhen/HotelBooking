using Facilities.Domain.Entities;
using Facilities.Infrastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Facilities.Infrastructure;

public class FacilitiesDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    public FacilitiesDbContext(DbContextOptions<FacilitiesDbContext> options,
        ILoggerFactory loggerFactory)
        : base(options)
    {
    }

    public DbSet<FacilityTree> Facilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        
        modelBuilder.ApplyConfiguration(new FacilityEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
        
    }
}