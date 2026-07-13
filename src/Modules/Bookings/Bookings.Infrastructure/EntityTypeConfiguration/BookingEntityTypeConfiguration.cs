using Bookings.Domain.Entities;
using Bookings.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.EntityTypeConfiguration;

public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings", "Bookings");
        builder.HasKey(x => x.BookingId);
        
        builder.Property(x => x.HotelId).IsRequired();
        builder.Property(x => x.RoomId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.GuestsCount).IsRequired();
        builder.Property(x => x.SpecialRequest).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ConfirmedAt);
        builder.Property(x => x.CanceledAt);
        builder.Property(x => x.CancelledBy).HasConversion<int?>();
        builder.Property(x => x.CancellationReason).HasMaxLength(500);

        builder.ComplexProperty<Money>(x => x.TotalPrice, m =>
        {
            m.Property(p => p.Amount)
                .HasColumnName("TotalPrice_Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            m.Property(p => p.Currency)
                .HasColumnName("TotalPrice_Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.ComplexProperty<DateRange>(x => x.BookingDates, d =>
        {
            d.Property(p => p.Start)
                .HasColumnName("CheckIn")
                .IsRequired();
            d.Property(p => p.End)
                .HasColumnName("CheckOut")
                .IsRequired();
        });

        builder.ComplexProperty<GuestInfo>(x => x.GuestInfo, g =>
        {
            g.Property(p => p.FirstName)
                .HasColumnName("GuestFirstName")
                .HasMaxLength(100)
                .IsRequired();
            g.Property(p => p.LastName)
                .HasColumnName("GuestLastName")
                .HasMaxLength(100)
                .IsRequired();
            g.Property(p => p.Email)
                .HasColumnName("GuestEmail")
                .HasMaxLength(254)
                .IsRequired();
            g.Property(p => p.PhoneNumber)
                .HasColumnName("GuestPhoneNumber")
                .HasMaxLength(20)
                .IsRequired();
        });
    }
}