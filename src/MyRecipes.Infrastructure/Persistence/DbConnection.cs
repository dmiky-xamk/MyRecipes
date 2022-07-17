using Microsoft.Extensions.Configuration;

namespace MyRecipes.Infrastructure.Persistence;

internal class DbConnection
{
    private readonly IConfiguration _config;

    public DbConnection(IConfiguration config)
    {
        _config = config;
    }
}