using Application.Outbox;

namespace Users.Infrastructure.Configuration.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly UsersDbContext _dbContext;
    
    public OutboxAccessor(UsersDbContext dbContext)
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