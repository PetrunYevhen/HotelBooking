using Application.Events;
using Application.Outbox;
using Dapper;
using Infrastructure.Data;
using Infrastructure.DomainEventDispatching;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Payments.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : IRequestHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    private readonly IDomainEventNotificationMapper _domainEventNotificationMapper;

    public ProcessOutboxCommandHandler(IMediator mediator, INpgsqlConnectionFactory npgsqlConnectionFactory, IDomainEventNotificationMapper domainEventNotificationMapper)
    {
        _mediator = mediator;
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
        _domainEventNotificationMapper = domainEventNotificationMapper;
    }

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = $"""
                            SELECT 
                                "OutboxMessage"."Id" AS "{nameof(OutboxMessage.Id)}", 
                                "OutboxMessage"."Type" AS "{nameof(OutboxMessage.Type)}", 
                                "OutboxMessage"."Data" AS "{nameof(OutboxMessage.Data)}" 
                            FROM "Payments"."OutboxMessages" AS "OutboxMessage" 
                            WHERE "OutboxMessage"."ProcessedDate" IS NULL 
                            ORDER BY "OutboxMessage"."OccurredOn"
                            LIMIT 100
                            """;

        var messages = await connection.QueryAsync<OutboxMessage>(sql);
        var messagesList = messages.AsList();

        const string sqlUpdateProcessedDate = """
                                              UPDATE "Payments"."OutboxMessages" 
                                              SET "ProcessedDate" = @Date 
                                              WHERE "Id" = @Id
                                              """;
        if (messagesList.Count > 0)
        {
            foreach (var message in messagesList)
            {
                var type = _domainEventNotificationMapper.GetType(message.Type);
                var @event = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;
                
                using (LogContext.Push(new OutboxMessageContextEnricher(@event)))
                {
                    await _mediator.Publish(@event, cancellationToken);

                    await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                    {
                        Date = DateTime.UtcNow,
                        message.Id
                    });
                }
            }
        }
    }

    private class OutboxMessageContextEnricher : ILogEventEnricher
    {
        private readonly IDomainEventNotification _notification;

        public OutboxMessageContextEnricher(IDomainEventNotification notification)
        {
            _notification = notification;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
        }
    }
}
