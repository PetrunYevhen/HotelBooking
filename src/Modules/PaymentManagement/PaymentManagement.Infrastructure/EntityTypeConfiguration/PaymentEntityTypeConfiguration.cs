using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Infrastructure.EntityTypeConfiguration;

public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "PaymentManagement");
        builder.HasKey(p => p.PaymentId);
        
    }
}