using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviews.Domain.Entities.Snapshot;

namespace Reviews.Infrastructure.EntityTypeConfiguration;

public class HotelRatingSnapshotEntityTypeConfiguration : IEntityTypeConfiguration<HotelRatingSnapshot>
{
    public void Configure(EntityTypeBuilder<HotelRatingSnapshot> builder)
    {
        builder.ToTable("HotelRatingSnapshots", "Reviews");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("Id").IsRequired();
        builder.Property(s => s.HotelId).HasColumnName("HotelId").IsRequired();
        builder.Property(s => s.AvgRating).HasColumnName("AvgRating").IsRequired();
        builder.Property(s => s.LastCalculatedAt).HasColumnName("LastCalculatedAt").IsRequired();

        builder.HasIndex(s => s.HotelId);
        builder.HasIndex(s => new { s.HotelId, s.LastCalculatedAt });
    }
}
