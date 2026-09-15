using SharedKernel.Contracts;
using Dapper;
using Infrastructure.Data;
using MediatR;
using Accommodations.Application.Contracts;

namespace Accommodations.Application.HotelierApplications;

public sealed class GetMyHotelierApplicationQuery(AccountId applicantId) : QueryBase<HotelierApplicationDto?>
{
    public AccountId ApplicantId { get; } = applicantId;
}

public sealed class GetMyHotelierApplicationQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetMyHotelierApplicationQuery, HotelierApplicationDto?>
{
    public async Task<HotelierApplicationDto?> Handle(GetMyHotelierApplicationQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT a."HotelierApplicationId", a."ApplicantId",
                a."LegalBusinessName", a."RegistrationNumber", a."TaxNumber", a."BusinessEmail", a."BusinessPhoneNumber",
                a."FirstPropertyName", a."FirstPropertyAddress", CASE a."Status" WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Rejected' END AS "Status", a."SubmittedAt", a."ReviewedAt",
                a."ReviewedByUserId", a."RejectionReason"
            FROM "Accommodations"."HotelierApplications" a
           
            WHERE a."ApplicantId" = @ApplicantId
            ORDER BY a."SubmittedAt" DESC LIMIT 1
            """;
        return await connection.QueryFirstOrDefaultAsync<HotelierApplicationDto>(new CommandDefinition(sql,
            new { ApplicantId = request.ApplicantId.Value }, cancellationToken: cancellationToken));
    }
}
