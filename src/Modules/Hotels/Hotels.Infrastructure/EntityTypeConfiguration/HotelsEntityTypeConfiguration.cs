using Hotels.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotels.Infastructure.EntityTypeConfiguration;

public class HotelsEntityTypeConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels", "HotelManagement");
        builder.HasKey(h => h.HotelId);
    }
}