using Application.Outbox;

namespace PaymentManagement.Infrastructure.Configuration.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly PaymentDbContext _dbContext;
    
    public OutboxAccessor(PaymentDbContext dbContext)
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