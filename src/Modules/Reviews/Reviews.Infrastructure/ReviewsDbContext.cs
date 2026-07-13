using Application.Outbox;
using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reviews.Domain.Entities.EligibleReview;
using Reviews.Domain.Entities.Reviews;
using Reviews.Domain.Entities.Snapshot;

namespace Reviews.Infrastructure;

public class ReviewsDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }
    public DbSet<EligibleReview> EligibleReviews { get; set; }
    public DbSet<HotelRatingSnapshot> HotelRatings { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options, ILoggerFactory loggerFactory)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
