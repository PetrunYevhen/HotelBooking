using Facilities.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facilities.Infrastructure.EntityTypeConfiguration;

public class FacilityEntityTypeConfiguration: IEntityTypeConfiguration<FacilityTree>
{
    public void Configure(EntityTypeBuilder<FacilityTree> builder)
    {
        builder.ToTable("FacilityTree", "Facilities");
        builder.HasKey(key => key.FacilityTreeId);
        
        
        builder.Property(f => f.Path)
            .HasColumnType("ltree")
            .IsRequired();

        // Додатково можна вказати Name
        builder.Property(f => f.Name)
            .IsRequired();
    }
}