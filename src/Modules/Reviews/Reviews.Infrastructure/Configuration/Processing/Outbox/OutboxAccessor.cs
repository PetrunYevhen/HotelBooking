using Application.Outbox;

namespace Reviews.Infrastructure.Configuration.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly ReviewsDbContext _context;

    public OutboxAccessor(ReviewsDbContext context)
    {
        _context = context;
    }

    public void Add(OutboxMessage message)
    {
        _context.OutboxMessages.Add(message);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
