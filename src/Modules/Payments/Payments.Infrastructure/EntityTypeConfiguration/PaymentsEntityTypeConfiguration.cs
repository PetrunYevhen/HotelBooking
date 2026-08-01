using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;
using SharedKernel.ValueObjects;

namespace Payments.Infrastructure.EntityTypeConfiguration;

public class PaymentsEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "Payments");
        builder.HasKey(p => p.PaymentId);
        builder.HasKey(p => p.PaymentId);
        
        builder.Property(p => p.BookingId)
            .IsRequired();

        builder.ComplexProperty<Money>(p => p.TotalAmount, b =>
        {
            b.Property(m => m.Amount)
                .HasColumnName("Amount")
                .IsRequired();

            b.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.ComplexProperty<Money>(p => p.RefundedAmount, b =>
        {
            b.Property(m => m.Amount)
                .HasColumnName("RefundedAmount_Amount")
                .IsRequired();

            b.Property(m => m.Currency)
                .HasColumnName("RefundedAmount_Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.ExternalTransactionId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.CompletedAt);

        builder.HasIndex(p => p.BookingId).IsUnique();
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.ExternalTransactionId).IsUnique();
    }
}