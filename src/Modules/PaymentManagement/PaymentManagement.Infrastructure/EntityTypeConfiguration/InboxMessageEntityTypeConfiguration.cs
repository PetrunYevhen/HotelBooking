using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentManagement.Infrastructure.EntityTypeConfiguration;

    public class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages", "PaymentManagement"); 
        
            builder.HasKey(x => x.Id);
        }
    }
