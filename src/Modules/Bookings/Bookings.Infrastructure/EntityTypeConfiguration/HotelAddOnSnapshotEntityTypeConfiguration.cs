using Bookings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.EntityTypeConfiguration;

public sealed class HotelAddOnSnapshotEntityTypeConfiguration : IEntityTypeConfiguration<HotelAddOnSnapshot>
{
    public void Configure(EntityTypeBuilder<HotelAddOnSnapshot> builder)
    {
        builder.ToTable("HotelAddOnSnapshots", "Bookings");
        builder.HasKey(x => x.HotelAddOnId);
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.PricingType).HasConversion<int>().IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasIndex(x => x.HotelId);
        builder.ComplexProperty<Money>(x => x.Price, money =>
        {
            money.Property(x => x.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(x => x.Currency).HasColumnName("Price_Currency").HasMaxLength(3).IsRequired();
        });
    }
}
