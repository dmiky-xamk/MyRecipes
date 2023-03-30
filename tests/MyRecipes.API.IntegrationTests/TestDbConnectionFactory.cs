using System.Data;
using MyRecipes.Application.Infrastructure.Persistence;
using Npgsql;

namespace MyRecipes.API.IntegrationTests;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    public string ConnectionString { get; }
    
    public TestDbConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(ConnectionString);

        await connection.OpenAsync();

        return connection;
    }
}