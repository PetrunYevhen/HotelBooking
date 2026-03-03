using System.Data;
using Infrastructure.Data;
using Npgsql;

namespace Infrastructure;

public class NpgsqlConnectionFactory : INpgsqlConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
    
    public string GetConnectionString()
    {
        return _connectionString;
    }
}