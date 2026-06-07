using Accommodations.Domain.Entities.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accommodations.Infrastructure.EntityTypeConfiguration;

public class PricingEntityTypeConfiguration : IEntityTypeConfiguration<Pricing>
{
    public void Configure(EntityTypeBuilder<Pricing> builder)
    {
        builder.ToTable("Pricings", "Accommodations");

        builder.HasKey(p => p.PricingId);
        builder.Property(p => p.RoomId).IsRequired();

        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.Type).HasConversion<int>().IsRequired();

        builder.ComplexProperty(p => p.Price, m =>
        {
            m.Property(x => x.Amount).IsRequired().HasColumnName("Price_Amount").HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).IsRequired().HasColumnName("Price_Currency").HasMaxLength(3);
        });

        builder.ComplexProperty(p => p.ValidDates, d =>
        {
            d.Property(x => x.Start).IsRequired().HasColumnName("ValidFrom").HasColumnType("timestamptz");
            d.Property(x => x.End).IsRequired().HasColumnName("ValidTo").HasColumnType("timestamptz");
        });

        builder.HasIndex(p => new { p.RoomId, p.IsActive });
        builder.HasIndex(p => p.RoomId);
    }
}