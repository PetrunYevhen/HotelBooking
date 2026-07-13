using Dapper;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using MediatR;
using Newtonsoft.Json;

namespace Notifications.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler : IRequestHandler<ProcessInboxCommand>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessInboxCommandHandler(
        INpgsqlConnectionFactory npgsqlConnectionFactory,
        IMediator mediator,
        IUnitOfWork unitOfWork)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = $""""
                            SELECT
                                "InboxMessages"."Id" AS "{nameof(InboxMessageDto.Id)}",
                                "InboxMessages"."Type" AS "{nameof(InboxMessageDto.Type)}",
                                "InboxMessages"."Data" AS "{nameof(InboxMessageDto.Data)}"
                            FROM "Notifications"."InboxMessages" AS "InboxMessages"
                            WHERE "InboxMessages"."ProcessedDate" IS NULL
                            ORDER BY "InboxMessages"."OccurredOn"
                            LIMIT 100
                            """";

        var messages = await connection.QueryAsync<InboxMessageDto>(sql);

        const string sqlUpdateProcessedDate = """
                                              UPDATE "Notifications"."InboxMessages"
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
                Console.WriteLine($"[Inbox] Type not found: {message.Type}");
                continue;
            }

            var request = JsonConvert.DeserializeObject(message.Data, type);

            try
            {
                await _mediator.Publish(request!, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
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
