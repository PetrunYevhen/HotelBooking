using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.EntityTypeConfiguration;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications", "Notifications");

        builder.HasKey(n => n.NotificationId);
        
        builder.Property(n => n.UserId)
            .IsRequired();

        builder.Property(n => n.RecipientEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(n => n.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(n => n.Subject)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(n => n.Content)
            .IsRequired();

        builder.Property(n => n.FailureReason)
            .HasMaxLength(1024);

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.Property(n => n.SentAt);
    }
}