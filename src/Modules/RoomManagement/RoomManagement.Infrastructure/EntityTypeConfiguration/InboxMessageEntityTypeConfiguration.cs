using Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RoomManagement.Infrastructure.EntityTypeConfiguration;

    public class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages", "RoomManagement"); 
        
            builder.HasKey(x => x.Id);
        }
    }
