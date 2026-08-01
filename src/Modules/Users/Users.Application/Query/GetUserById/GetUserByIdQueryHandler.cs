using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Users.Application.Query.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetUserByIdQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                           SELECT
                               "UserId",
                               "Username",
                               "Email",
                               "FirstName",
                               "LastName",
                               "PhoneNumber",
                               CASE "Role"
                                   WHEN 1 THEN 'Admin'
                                   WHEN 2 THEN 'User'
                                   WHEN 3 THEN 'Moderator'
                                   ELSE 'Unknown'
                               END AS "Role"
                           FROM "Accounts"."Users"
                           WHERE "UserId" = @UserId
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserDto>(new CommandDefinition(
            sql,
            new { UserId = request.UserId.Value },
            cancellationToken: cancellationToken));
    }
}
