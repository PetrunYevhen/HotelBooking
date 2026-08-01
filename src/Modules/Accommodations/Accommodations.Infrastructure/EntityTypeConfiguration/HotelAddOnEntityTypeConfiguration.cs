using Accommodations.Domain.Entities.HotelAddOns;
using Accommodations.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Accommodations.Infrastructure.EntityTypeConfiguration;

public sealed class HotelAddOnEntityTypeConfiguration : IEntityTypeConfiguration<HotelAddOn>
{
    public void Configure(EntityTypeBuilder<HotelAddOn> builder)
    {
        builder.ToTable("HotelAddOns", "Accommodations");
        builder.HasKey(x => x.HotelAddOnId);
        builder.Property(x => x.HotelAddOnId)
            .HasConversion(id => id.Value, value => new HotelAddOnId(value))
            .ValueGeneratedNever();
        builder.Property(x => x.HotelId)
            .HasConversion(id => id.Value, value => new HotelId(value))
            .IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.PricingType).HasConversion<int>().IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => new { x.HotelId, x.Code }).IsUnique();

        builder.ComplexProperty<Money>(x => x.Price, money =>
        {
            money.Property(x => x.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(x => x.Currency).HasColumnName("Price_Currency").HasMaxLength(3).IsRequired();
        });
    }
}
