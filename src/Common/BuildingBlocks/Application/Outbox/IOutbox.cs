namespace Application.Outbox;

public interface IOutbox
{
    void Add(OutboxMessage message);
    Task Save (CancellationToken cancellationToken);
}