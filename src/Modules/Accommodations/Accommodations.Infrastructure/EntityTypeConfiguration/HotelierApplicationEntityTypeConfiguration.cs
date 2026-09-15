using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Accommodations.Domain.Entities.HotelierApplications;

namespace Accommodations.Infrastructure.EntityTypeConfiguration;

public sealed class HotelierApplicationEntityTypeConfiguration : IEntityTypeConfiguration<HotelierApplication>
{
    public void Configure(EntityTypeBuilder<HotelierApplication> builder)
    {
        builder.ToTable("HotelierApplications", "Accommodations");
        builder.HasKey(x => x.HotelierApplicationId);
        builder.Property(x => x.HotelierApplicationId).HasConversion(x => x.Value, x => new HotelierApplicationId(x));
        builder.Property(x => x.ApplicantId).HasConversion(x => x.Value, x => new SharedKernel.Contracts.AccountId(x));
        builder.Property(x => x.ReviewedByUserId).HasConversion(x => x == null ? (Guid?)null : x.Value,
            x => x == null ? null : new SharedKernel.Contracts.AccountId(x.Value));
        builder.Property<uint>("Version").IsRowVersion();
        builder.Property(x => x.LegalBusinessName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.RegistrationNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TaxNumber).HasMaxLength(100);
        builder.Property(x => x.BusinessEmail).HasMaxLength(320).IsRequired();
        builder.Property(x => x.BusinessPhoneNumber).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FirstPropertyName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FirstPropertyAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.RejectionReason).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ApplicantId, x.Status }).IsUnique().HasFilter("\"Status\" = 0");
    }
}
