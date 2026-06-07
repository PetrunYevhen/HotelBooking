using Accommodations.Domain.Entities.Rooms.Facility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accommodations.Infrastructure.EntityTypeConfiguration.Room;

public class RoomFacilityEntityTypeConfiguration : IEntityTypeConfiguration<RoomFacility>
{
    public void Configure(EntityTypeBuilder<RoomFacility> builder)
    {
        builder.ToTable("RoomFacilities", "Accommodations");

        builder.HasKey(f => f.RoomFacilityId);
        builder.Property(f => f.RoomId).IsRequired();

        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Category).HasConversion<int>().IsRequired();

        builder.HasIndex(f => f.RoomId);
        builder.HasIndex(f => f.Category);
    }
}