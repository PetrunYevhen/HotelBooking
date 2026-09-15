using Dapper;
using Infrastructure.Data;
using MediatR;
using Accommodations.Application.Contracts;

namespace Accommodations.Application.HotelierApplications;

public sealed class GetHotelierApplicationsQuery(string? status) : QueryBase<IReadOnlyList<HotelierApplicationDto>>
{
    public string? Status { get; } = status;
}

public sealed class GetHotelierApplicationsQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetHotelierApplicationsQuery, IReadOnlyList<HotelierApplicationDto>>
{
    public async Task<IReadOnlyList<HotelierApplicationDto>> Handle(GetHotelierApplicationsQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT a."HotelierApplicationId", a."ApplicantId",
                a."LegalBusinessName", a."RegistrationNumber", a."TaxNumber", a."BusinessEmail", a."BusinessPhoneNumber",
                a."FirstPropertyName", a."FirstPropertyAddress", CASE a."Status" WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Rejected' END AS "Status", a."SubmittedAt", a."ReviewedAt",
                a."ReviewedByUserId", a."RejectionReason"
            FROM "Accommodations"."HotelierApplications" a
            WHERE @Status IS NULL OR CASE a."Status" WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Rejected' END = @Status
            ORDER BY a."SubmittedAt" ASC
            """;
        var results = await connection.QueryAsync<HotelierApplicationDto>(new CommandDefinition(sql,
            new { Status = request.Status }, cancellationToken: cancellationToken));
        return results.ToList();
    }
}
