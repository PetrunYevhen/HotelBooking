using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.HotelRelations;

namespace HotelManagement.Infastructure.EntityTypeConfiguration;

public class HotelRoomsEntityTypeConfiguration : IEntityTypeConfiguration<HotelRooms>
{
    public void Configure(EntityTypeBuilder<HotelRooms> builder)
    {
        builder.ToTable("HotelRooms", "Shared"); 
        builder.HasKey(key => new { key.HotelId, key.RoomId });
        
    }
}