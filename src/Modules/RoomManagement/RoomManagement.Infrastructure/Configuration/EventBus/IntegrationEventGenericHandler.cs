using Autofac;
using Dapper;
using Infrastructure.Data;
using Infrastructure.EventBus;
using Infrastructure.Serialization;
using Newtonsoft.Json;

namespace RoomManagement.Infrastructure.Configuration.EventBus;

public class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
    where T : global::Infrastructure.EventBus.IntegrationEvent
{
    public async Task Handle(T @event, CancellationToken cancellationToken = default)
    {
        using (var scope = RoomManagementCompositoryRoot.BeginLifetimeScope())
        {
            using (var connection = scope.Resolve<INpgsqlConnectionFactory>().CreateConnection())
            {
                string type = @event.GetType().FullName;
                var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });

                var sql = "INSERT INTO \"meetings\".\"InboxMessages\" (\"Id\", \"OccurredOn\", \"Type\", \"Data\") " +
                          "VALUES (@Id, @OccurredOn, @Type, @Data)";

                await connection.ExecuteScalarAsync(sql, new
                {
                    @event.Id,
                    @event.OccuredOn,
                    type,
                    data
                });
            }
        }
    }
}