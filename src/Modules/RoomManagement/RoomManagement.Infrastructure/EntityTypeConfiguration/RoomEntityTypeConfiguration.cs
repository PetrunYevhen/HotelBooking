using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomManagment.Domain.Entities;

namespace RoomManagment.Infrastructure.EntityTypeConfiguration;

public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
{
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "RoomManagement");
        builder.HasKey(r => r.RoomId);
    }
}