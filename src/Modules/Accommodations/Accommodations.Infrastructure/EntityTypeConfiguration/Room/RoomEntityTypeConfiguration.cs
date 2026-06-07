using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accommodations.Infrastructure.EntityTypeConfiguration.Room;

public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Rooms.Room>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Rooms.Room> builder)
    {
        builder.ToTable("Rooms", "Accommodations");

        builder.HasKey(r => r.RoomId);

        builder.Property(r => r.RoomNumber).IsRequired().HasMaxLength(20);
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.Beds).IsRequired();
        builder.Property(r => r.Capacity).IsRequired();
        builder.Property(r => r.IsActive).IsRequired();
        builder.Property(r => r.Status).HasConversion<int>().IsRequired();
        builder.Property(r => r.Type).HasConversion<int>().IsRequired();

        builder.ComplexProperty(r => r.BasePrice, m =>
        {
            m.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).IsRequired().HasColumnName("BasePrice_Currency").HasMaxLength(3);
        });

        builder.HasMany(r => r.Facilities)
            .WithOne()
            .HasForeignKey(f => f.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();
        builder.HasIndex(r => r.HotelId);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.IsActive);
    }
}