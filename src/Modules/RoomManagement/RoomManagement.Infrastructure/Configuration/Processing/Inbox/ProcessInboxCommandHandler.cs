using Dapper;
using Infrastructure.Data;
using MediatR;
using Newtonsoft.Json;

namespace RoomManagement.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler : IRequestHandler<ProcessInboxCommand>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    private readonly IMediator _mediator;


    public ProcessInboxCommandHandler(INpgsqlConnectionFactory npgsqlConnectionFactory, IMediator mediator)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
        _mediator = mediator;
    }

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        const string sql = $""""
                            SELECT 
                                "InboxMessages"."Id" AS "{nameof(InboxMessageDto.Id)}",
                                "InboxMessages"."Type" AS "{nameof(InboxMessageDto.Type)}",
                                "InboxMessages"."Data" AS "{nameof(InboxMessageDto.Data)}"
                            FROM "RoomManagement"."InboxMessages" AS "InboxMessages"
                            WHERE "InboxMessages"."ProcessedDate" IS NULL 
                            ORDER BY "InboxMessages"."OccurredOn"
                            """";

        var messages = await connection.QueryAsync<InboxMessageDto>(sql);

        const string sqlUpdateProcessedDate = """
                                                  UPDATE "RoomManagement"."InboxMessages" 
                                                  SET "ProcessedDate" = @Date 
                                                  WHERE "Id" = @Id
                                              """;

        foreach (var message in messages)
        {
            Type? type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType(message.Type))
                .FirstOrDefault(t => t != null);
            
            if (type == null)
            {
                Console.WriteLine($"[Inbox] ПОМИЛКА: Не знайдено тип {message.Type}");
                continue; 
            }
            
            var request = JsonConvert.DeserializeObject(message.Data, type);

            try
            {
                await _mediator.Publish((INotification)request, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await connection.ExecuteScalarAsync(sqlUpdateProcessedDate, new
            {
                Date = DateTime.UtcNow,
                message.Id
            });
        }
    }
}