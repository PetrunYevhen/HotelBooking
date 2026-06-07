using System.Data;

namespace Infrastructure.Data;

public interface INpgsqlConnectionFactory
{
    IDbConnection CreateNewConnection();
    string GetConnectionString();

}