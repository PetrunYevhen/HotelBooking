using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.EntityTypeConfiguration;

public class UsersEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "Accounts");
        builder.HasKey(p => p.UserId);
        builder.Property(p => p.Username)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(p => p.Username)
            .IsUnique();
        builder.Property(p => p.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(p => p.Email)
            .HasMaxLength(320)
            .IsRequired();
        builder.HasIndex(p => p.Email)
            .IsUnique();
        builder.Property(p => p.Role)
            .IsRequired();
        builder.Property(p => p.RefreshTokenHash)
            .HasMaxLength(128);

        builder.ComplexProperty(p => p.PersonalInfo, personalInfo =>
        {
            personalInfo.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();
            personalInfo.Property(p => p.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
            personalInfo.Property(p => p.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(16)
                .IsRequired();
        });
    }
}
