using Dapper;
using Facilities.Domain.Entities;
using Facilities.Domain.RepositoryContract;
using Infrastructure.Data;
using Serilog;

namespace Facilities.Infrastructure.Repositories;

public class FacilityReadRepository : IFacilityReadRepository
{
private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
private readonly ILogger _logger;

public FacilityReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory, ILogger logger)
{
    _npgsqlConnectionFactory = npgsqlConnectionFactory;
    _logger = logger;
}

public async Task<List<FacilityTree>> GetAllFacilitiesAsync()
{  
    using var connection = _npgsqlConnectionFactory.CreateConnection();
    
    const string sql = @"
        SELECT ""FacilityTreeId"", ""Name"", ""Path"" 
        FROM ""Facilities"".""FacilityTree""";
    var result = await connection.QueryAsync<FacilityTree>(sql);

    return result.ToList();
}

}