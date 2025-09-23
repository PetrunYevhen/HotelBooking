using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.HotelRelations;

namespace HotelManagement.Infastructure.EntityTypeConfiguration;

public class HotelFacilityEntityTypeConfiguration : IEntityTypeConfiguration<HotelFacilities>
{
    public void Configure(EntityTypeBuilder<HotelFacilities> builder)
    {
        builder.ToTable("HotelFacilities", "Shared");
        builder.HasKey (key => new { key.HotelId, key.FacilityId });
    }
}