using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviews.Domain.Entities.Reviews;

namespace Reviews.Infrastructure.EntityTypeConfiguration;

public class ReviewsEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", "Reviews");
        builder.HasKey(r => r.ReviewId);

        builder.Property(r => r.ReviewId).HasColumnName("ReviewId");
        builder.Property(r => r.BookingId).HasColumnName("BookingId").IsRequired();
        builder.Property(r => r.HotelId).HasColumnName("HotelId").IsRequired();
        builder.Property(r => r.UserId).HasColumnName("UserId").IsRequired();

        builder.OwnsOne(r => r.Rating, rating =>
        {
            rating.Property(x => x.Value).HasColumnName("Rating").IsRequired();
        });

        builder.Property(r => r.Title).HasColumnName("Title").HasMaxLength(100);
        builder.Property(r => r.Comment).HasColumnName("Comment").HasMaxLength(2000);
        builder.Property(r => r.Status).HasColumnName("Status").HasConversion<int>().IsRequired();
        builder.Property(r => r.CreatedAt).HasColumnName("CreatedAt").IsRequired();
        builder.Property(r => r.PublishedAt).HasColumnName("PublishedAt");
        builder.Property(r => r.RejectedAt).HasColumnName("RejectedAt");
        builder.Property(r => r.RejectionReason).HasColumnName("RejectionReason").HasMaxLength(500);
        builder.Property(r => r.IsBookingVerified).HasColumnName("IsBookingVerified").IsRequired();
    }
}