using Autofac;
using Dapper;
using Infrastructure.Data;
using Infrastructure.Inbox;
using Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Accommodations.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler(INpgsqlConnectionFactory connections, ILifetimeScope scope, ILogger logger)
    : IRequestHandler<ProcessInboxCommand>
{
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        using var connection = connections.CreateNewConnection();
        var ids = await connection.QueryAsync<Guid>(new CommandDefinition(
            """SELECT "Id" FROM "Accommodations"."InboxMessages" WHERE "ProcessedDate" IS NULL ORDER BY "OccurredOn" LIMIT 100""",
            cancellationToken: cancellationToken));
        foreach (var id in ids)
        {
            await using var messageScope = scope.BeginLifetimeScope();
            var context = messageScope.Resolve<AccommodationsDbContext>();
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var messages = await context.Set<InboxMessage>().FromSqlInterpolated(
                    $"""SELECT * FROM "Accommodations"."InboxMessages" WHERE "Id" = {id} AND "ProcessedDate" IS NULL FOR UPDATE SKIP LOCKED""")
                    .ToListAsync(cancellationToken);
                var message = messages.SingleOrDefault();
                if (message is null) continue;
                var type = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(message.Type))
                    .FirstOrDefault(t => t is not null) ?? throw new InvalidOperationException($"Unknown inbox type {message.Type}");
                var notification = JsonConvert.DeserializeObject(message.Data, type) as INotification
                    ?? throw new InvalidOperationException($"Invalid inbox message {id}");
                await messageScope.Resolve<IMediator>().Publish(notification, cancellationToken);
                message.ProcessedDate = DateTime.UtcNow;
                await messageScope.Resolve<IUnitOfWork>().CommitAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
            catch (Exception exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.Error(exception, "Inbox message {MessageId} failed; it will be retried", id);
            }
        }
    }
}
