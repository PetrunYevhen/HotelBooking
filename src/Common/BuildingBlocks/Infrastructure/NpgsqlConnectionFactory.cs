using System.Data;
using Infrastructure.Data;
using Npgsql;

namespace Infrastructure;

public class NpgsqlConnectionFactory : INpgsqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        
    }

    public IDbConnection CreateNewConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
    
    public string GetConnectionString()
    {
        return _connectionString;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Dispose();
        }
        
    }
}