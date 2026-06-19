using Application.Events;
using Application.Outbox;
using Dapper;
using Infrastructure.Data;
using Infrastructure.DomainEventDispatching;
using Infrastructure.UnitOfWork;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Accommodations.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : IRequestHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    private readonly IDomainEventNotificationMapper _domainEventNotificationMapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessOutboxCommandHandler(IMediator mediator, INpgsqlConnectionFactory npgsqlConnectionFactory, IDomainEventNotificationMapper domainEventNotificationMapper, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
        _domainEventNotificationMapper = domainEventNotificationMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = $"""
                            SELECT 
                                "OutboxMessage"."Id" AS "{nameof(OutboxMessage.Id)}", 
                                "OutboxMessage"."Type" AS "{nameof(OutboxMessage.Type)}", 
                                "OutboxMessage"."Data" AS "{nameof(OutboxMessage.Data)}" 
                            FROM "Accommodations"."OutboxMessages" AS "OutboxMessage" 
                            WHERE "OutboxMessage"."ProcessedDate" IS NULL 
                            ORDER BY "OutboxMessage"."OccurredOn"
                            """;

        var messages = await connection.QueryAsync<OutboxMessage>(sql);
        var messagesList = messages.AsList();

        const string sqlUpdateProcessedDate = """
                                              UPDATE "Accommodations"."OutboxMessages" 
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
                    try
                    {
                        await _mediator.Publish(@event, cancellationToken);
                        await _unitOfWork.CommitAsync(cancellationToken);
                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new { Date = DateTime.UtcNow, message.Id });
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Outbox message {MessageId} failed: {Error}", message.Id, ex.Message);
                        throw;
                    }
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