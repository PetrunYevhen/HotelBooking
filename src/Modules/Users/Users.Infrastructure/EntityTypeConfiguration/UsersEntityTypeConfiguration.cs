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
