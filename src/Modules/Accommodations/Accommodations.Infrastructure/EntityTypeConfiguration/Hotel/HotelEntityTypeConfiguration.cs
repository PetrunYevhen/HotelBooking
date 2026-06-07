using Accommodations.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Accommodations.Infrastructure.EntityTypeConfiguration;

public class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels", "Accommodations");
        builder.HasKey(h => h.HotelId);
        
        builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
        builder.Property(h => h.Description).IsRequired().HasMaxLength(500);
        builder.ComplexProperty<Address>(h => h.Address, address =>
        {
            address.Property(p => p.Country).IsRequired().HasColumnName("Country").HasMaxLength(100);
            address.Property(p => p.City).IsRequired().HasColumnName("City").HasMaxLength(100);
            address.Property(p => p.Street).IsRequired().HasColumnName("Street").HasMaxLength(200);
            address.Property(p => p.PostalCode).IsRequired().HasColumnName("PostalCode").HasMaxLength(10);
        });
        builder.ComplexProperty<OperatingHours>(h => h.OperatingHours, hours =>
        {
            hours.Property(p => p.CheckIn).IsRequired().HasColumnName("CheckIn").HasMaxLength(100);
            hours.Property(p => p.CheckOut).IsRequired().HasColumnName("CheckOut").HasMaxLength(100);
        });
        builder.Property(h => h.Rating).IsRequired().HasColumnName("Rating").HasMaxLength(100);
        builder.ComplexProperty<Money>(h => h.MinRoomPrice, money =>
        {
            money.Property(p => p.Amount).IsRequired().HasColumnName("Amount").HasColumnType("decimal(18,2)");
            money.Property(p => p.Currency).IsRequired().HasColumnName("Currency").HasMaxLength(3);
        });
        builder.Property(h => h.Status).HasConversion<string>().IsRequired();
        
        builder.OwnsOne(h => h.MinRoomPrice, m =>
        {
            m.Property(p => p.Amount).HasColumnName("MinRoomPrice_Amount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("MinRoomPrice_Currency").HasMaxLength(3);
        });

        builder.HasMany(h => h.HotelFacilities)
            .WithOne()
            .HasForeignKey(f => f.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(h => h.Status);
        builder.HasIndex(h => h.Rating);
        builder.HasIndex("City");
    }
    
}