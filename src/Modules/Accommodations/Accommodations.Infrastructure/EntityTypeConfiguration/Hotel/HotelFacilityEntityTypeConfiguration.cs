using Accommodations.Domain.Entities.Hotels.Facility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accommodations.Infrastructure.EntityTypeConfiguration.Hotel;

public class HotelFacilityEntityTypeConfiguration : IEntityTypeConfiguration<HotelFacility>
{
    public void Configure(EntityTypeBuilder<HotelFacility> builder)
    {
        builder.ToTable("HotelFacilities", "Accommodations");

        builder.HasKey(f => f.HotelFacilityId);

        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Category).HasConversion<int>().IsRequired();

        builder.HasIndex(f => f.HotelId);
        builder.HasIndex(f => f.Category);
    }
}