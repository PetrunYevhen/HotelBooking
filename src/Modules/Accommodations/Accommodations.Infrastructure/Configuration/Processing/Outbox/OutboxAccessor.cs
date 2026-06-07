using Application.Outbox;

namespace Accommodations.Infrastructure.Configuration.Processing.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly  AccommodationsDbContext _context;

    public OutboxAccessor(AccommodationsDbContext context)
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