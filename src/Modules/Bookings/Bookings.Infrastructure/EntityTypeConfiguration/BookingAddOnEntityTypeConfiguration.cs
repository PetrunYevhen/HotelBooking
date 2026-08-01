using Bookings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.EntityTypeConfiguration;

public class BookingAddOnEntityTypeConfiguration : IEntityTypeConfiguration<BookingAddOn>
{
    public void Configure(EntityTypeBuilder<BookingAddOn> builder)
    {
        builder.ToTable("BookingAddOns", "Bookings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.HotelAddOnId);
        builder.Property(x => x.BookingId)
            .HasConversion(id => id.Value, value => new BookingId(value))
            .IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.PricingType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Quantity).IsRequired();

        builder.ComplexProperty<Money>(x => x.UnitPrice, money =>
        {
            money.Property(x => x.Amount).HasColumnName("UnitPrice_Amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(x => x.Currency).HasColumnName("UnitPrice_Currency").HasMaxLength(3).IsRequired();
        });
        builder.ComplexProperty<Money>(x => x.TotalPrice, money =>
        {
            money.Property(x => x.Amount).HasColumnName("TotalPrice_Amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(x => x.Currency).HasColumnName("TotalPrice_Currency").HasMaxLength(3).IsRequired();
        });

        builder.HasOne<Booking>()
            .WithMany(x => x.AddOns)
            .HasForeignKey(x => x.BookingId)
            .HasPrincipalKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
