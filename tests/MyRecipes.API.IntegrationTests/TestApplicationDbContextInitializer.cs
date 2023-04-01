using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Infrastructure.Persistence;

namespace MyRecipes.API.IntegrationTests;

/// <summary>
/// Used to initialize the test database.
/// </summary>
public class TestApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<TestApplicationDbContextInitializer> _logger;

    public TestApplicationDbContextInitializer(ILogger<TestApplicationDbContextInitializer> logger,
        ApplicationDbContext context, IDbConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // For identity tables.
            await _context.Database.MigrateAsync();

            // Domain tables.
            await CreateTables();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the test database.");
            throw;
        }
    }

    public async Task SeedAsync() => await Task.CompletedTask;

    private async Task CreateTables()
    {
        var queryRecipe = """
                CREATE TABLE IF NOT EXISTS recipe (
            	id Text NOT NULL,
            	name Text NOT NULL,
            	description Text NOT NULL DEFAULT '',
            	image Text NOT NULL DEFAULT '',
            	user_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES "AspNetUsers"("Id"));
            """;

        var queryIngredient = """
                CREATE TABLE IF NOT EXISTS ingredient (
            	id SERIAL,
            	name Text NOT NULL,
            	unit Text NOT NULL DEFAULT '',
            	amount Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        var queryDirection = """
                CREATE TABLE IF NOT EXISTS direction (
            	id SERIAL,
            	step Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            await connection.ExecuteAsync(queryRecipe);
            await connection.ExecuteAsync(queryIngredient);
            await connection.ExecuteAsync(queryDirection);
        }
    }
}