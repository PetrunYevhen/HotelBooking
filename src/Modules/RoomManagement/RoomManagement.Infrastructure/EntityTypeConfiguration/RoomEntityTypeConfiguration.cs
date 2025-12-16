using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomManagement.Domain.Entities;

namespace RoomManagement.Infrastructure.EntityTypeConfiguration;

public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
{
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "RoomManagement");
        builder.HasKey(r => r.RoomId);
        builder.Property(hotelid => hotelid.HotelId).IsRequired();
    }
}