using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MyRecipes.Application.Infrastructure.Persistence;

public sealed class PostgresConnectionFactory : IDbConnectionFactory
{
    public string ConnectionString { get; }

    public PostgresConnectionFactory(IConfiguration config)
    {
        ConnectionString = GetConnectionString(config);
    }
    
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(ConnectionString);

        await connection.OpenAsync();

        return connection;
    }

    /// <summary>
    /// Returns the connection string based on the environment variable.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns>The connection string.</returns>
    /// <exception cref="Exception">If no environment variable represents the defined environments.</exception>
    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") switch
        {
            
            "Development" => GetDevelopmentConnectionString(configuration),
            "Production" => GetProductionConnectionString(),
            _ => throw new Exception("ASPNETCORE_ENVIRONMENT was not set to Development or Production.")
        };

        return connectionString;
    }

    /// <summary>
    /// Returns the connection string from the configuration file.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>The connection string.</returns>
    /// <exception cref="Exception">If the <see cref="config.GetConnectionString(string)"/> doesn't find the connection string.</exception>
    private static string GetDevelopmentConnectionString(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Postgre");
        
        return connectionString ?? throw new Exception("Development environment connection string was null.");
    }

    /// <summary>
    /// Parses the connection string from the DATABASE_URL environment variable.
    /// </summary>
    /// <returns>The parsed connection string.</returns>
    /// <exception cref="Exception">If the <see cref="Environment.GetEnvironmentVariable(string)"/> doesn't find the DATABASE_URL environment variable.</exception>
    private static string GetProductionConnectionString()
    {
        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        
        if (connUrl is null)
        {
            throw new Exception("DATABASE_URL environment variable was null.");
        }

        // Parse connection URL to connection string for Npgsql
        connUrl = connUrl.Replace("postgres://", string.Empty);

        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];

        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];

        var connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";

        return connectionString;
    }
    
}