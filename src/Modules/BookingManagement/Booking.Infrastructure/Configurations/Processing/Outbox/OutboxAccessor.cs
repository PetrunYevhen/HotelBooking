using Application.Outbox;

namespace BookingManagement.Infrastructure.Configurations.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly BookingDbContext _dbContext;
    
    public OutboxAccessor(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }
    public void Add(OutboxMessage message)
    {
        _dbContext.OutboxMessages.Add(message);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}