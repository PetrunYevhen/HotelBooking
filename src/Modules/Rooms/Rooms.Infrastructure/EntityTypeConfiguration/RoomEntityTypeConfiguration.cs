using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.EntityTypeConfiguration;

public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
{
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "RoomManagement");
        builder.HasKey(r => r.RoomId);
        builder.Property(hotelId => hotelId.HotelId).IsRequired();
    }
}