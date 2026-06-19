using Autofac;
using Dapper;
using Infrastructure.Data;
using Infrastructure.EventBus;
using Infrastructure.Serialization;
using Newtonsoft.Json;

namespace Bookings.Infrastructure.Configurations.EventBus;

public class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T @event, CancellationToken cancellationToken = default)
    {
            using (var scope = BookingCompositoryRoot.BeginLifetimeScope())
            {
                using (var connection = scope.Resolve<INpgsqlConnectionFactory>().CreateNewConnection())
                {
                    string type = @event.GetType().FullName;
                    var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                    {
                        ContractResolver = new AllPropertiesContractResolver()
                    });

                    var sql = """
                        INSERT INTO "Bookings"."InboxMessages" ("Id", "OccurredOn", "Type", "Data")
                        VALUES (@Id, @OccurredOn, @Type, @Data::jsonb)
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
        
    }
}