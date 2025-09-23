using System.Data;

namespace Infrastructure.Data;

public interface INpgsqlConnectionFactory
{
    IDbConnection CreateConnection();
    // IDbConnection GetOpenConnection();
    string GetConnectionString();

}