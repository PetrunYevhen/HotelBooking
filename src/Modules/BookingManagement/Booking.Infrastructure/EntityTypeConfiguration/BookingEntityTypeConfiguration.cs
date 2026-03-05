using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingManagement.Infrastructure.EntityTypeConfiguration;

public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Booking>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Booking> builder)
    {
        builder.ToTable("Bookings", "BookingManagement");
        builder.HasKey(x => x.BookingId);
    }
}