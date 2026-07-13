using Accommodations.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accommodations.Infrastructure.EntityTypeConfiguration.Hotel;

public class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Hotels.Hotel>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Hotels.Hotel> builder)
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
            hours.Property(p => p.CheckIn).IsRequired().HasColumnName("OperatingHours_Start");
            hours.Property(p => p.CheckOut).IsRequired().HasColumnName("OperatingHours_End");
        });
        builder.Property(h => h.Rating).HasColumnName("Rating");
        builder.Property(h => h.Status).HasConversion<int>().IsRequired();

        builder.OwnsOne(h => h.MinRoomPrice, m =>
        {
            m.Property(p => p.Amount).HasColumnName("MinRoomPrice_Amount").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("MinRoomPrice_Currency").HasMaxLength(3);
        });
        
        builder.HasMany(h => h.HotelFacilities)
            .WithOne()
            .HasForeignKey(f => f.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.OwnsOne(h => h.Policies, p =>
        {
            p.Property(x => x.PetPolicy)
                .HasColumnName("PetPolicy").HasConversion<int>().IsRequired();
            p.Property(x => x.SmokingPolicy)
                .HasColumnName("SmokingPolicy").HasConversion<int>().IsRequired();
            p.OwnsOne(x => x.CancellationPolicy, c =>
            {
                c.Property(x => x.Type)
                    .HasColumnName("Policies_CancellationType").HasConversion<int>().IsRequired();
                c.Property(x => x.DeadlineDays)
                    .HasColumnName("Policies_Cancellation_DeadlineDays");
                c.Property(x => x.PercentagePenalty)
                    .HasColumnName("Policies_Cancellation_PenaltyPercentage");
            });
            p.OwnsOne(x => x.CheckOutHoursPolicy, c =>
            {
                c.Property(x => x.Hours)
                    .HasColumnName("CheckOutHoursPolicy").IsRequired();
            });
        });

        builder.HasIndex(h => h.Status);
        builder.HasIndex(h => h.Rating);
    }
    
}