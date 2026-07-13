using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviews.Domain.Entities.EligibleReview;

namespace Reviews.Infrastructure.EntityTypeConfiguration;

public class EligibleReviewEntityTypeConfiguration : IEntityTypeConfiguration<EligibleReview>
{
    public void Configure(EntityTypeBuilder<EligibleReview> builder)
    {
        builder.ToTable("EligibleReviews", "Reviews");
        builder.HasKey(e => e.BookingId);
        builder.Property(e => e.BookingId).HasColumnName("BookingId").IsRequired();
        builder.Property(e => e.HotelId).HasColumnName("HotelId").IsRequired();
        builder.Property(e => e.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(e => e.CompletedAt).HasColumnName("CompletedAt").IsRequired();
    }
}