using Autofac;
using Dapper;
using Infrastructure.Data;
using Infrastructure.EventBus;
using Infrastructure.Serialization;
using Newtonsoft.Json;

namespace Accommodations.Infrastructure.Configuration.EventBus;

public class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T @event, CancellationToken cancellationToken = default)
    {
        using (var scope = AccommodationsCompositionRoot.BeginLifetimeScope())
        {
            using (var connection = scope.Resolve<INpgsqlConnectionFactory>().CreateNewConnection())
            {
                string type = @event.GetType().FullName;
                var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });

                var sql = "INSERT INTO [meetings].[InboxMessages] (Id, OccurredOn, Type, Data) " +
                          "VALUES (@Id, @OccurredOn, @Type, @Data)";

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