using Application.Outbox;

namespace HotelManagement.Infastructure.Outbox;

public class OutboxAccessor : IOutbox
{
    private readonly  HotelDbContext _context;

    public OutboxAccessor(HotelDbContext context)
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