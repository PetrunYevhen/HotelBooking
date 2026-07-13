using Autofac;
using Dapper;
using Infrastructure.Data;
using Infrastructure.EventBus;
using Infrastructure.Serialization;
using Newtonsoft.Json;

namespace Notifications.Infrastructure.Configuration.EventBus;

public class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T @event, CancellationToken cancellationToken = default)
    {
        using var scope = NotificationsCompositionRoot.BeginLifetimeScope();
        using var connection = scope.Resolve<INpgsqlConnectionFactory>().CreateNewConnection();

        string type = @event.GetType().FullName;
        var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });

        const string sql = """
                           INSERT INTO "Notifications"."InboxMessages" ("Id", "OccurredOn", "Type", "Data")
                           VALUES (@Id, @OccurredOn, @Type, @Data)
                           ON CONFLICT ("Id") DO NOTHING
                           """;

        await connection.ExecuteScalarAsync(sql, new
        {
            @event.Id,
            @event.OccurredOn,
            type,
            data
        });
    }
}
