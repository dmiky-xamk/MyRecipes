using System.Data;

namespace MyRecipes.Application.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
    string ConnectionString { get; }
}