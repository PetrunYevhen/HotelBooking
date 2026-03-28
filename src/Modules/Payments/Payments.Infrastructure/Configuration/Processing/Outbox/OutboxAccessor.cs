using Application.Outbox;

namespace Payments.Infrastructure.Configuration.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly PaymentsDbContext _dbContext;
    
    public OutboxAccessor(PaymentsDbContext dbContext)
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