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
    private readonly ILogger<TestApplicationDbContextInitializer> _logger;
    private readonly IDataAccess _dataAccess;

    public TestApplicationDbContextInitializer(ILogger<TestApplicationDbContextInitializer> logger,
        ApplicationDbContext context, IDataAccess dataAccess)
    {
        _context = context;
        _logger = logger;
        _dataAccess = dataAccess;
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
        var sqlRecipe = """
                CREATE TABLE IF NOT EXISTS recipe (
            	id Text NOT NULL,
            	name Text NOT NULL,
            	description Text NOT NULL DEFAULT '',
            	image Text NOT NULL DEFAULT '',
            	user_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES "AspNetUsers"("Id"));
            """;

        var sqlIngredient = """
                CREATE TABLE IF NOT EXISTS ingredient (
            	id SERIAL,
            	name Text NOT NULL,
            	unit Text NOT NULL DEFAULT '',
            	amount Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        var sqlDirection = """
                CREATE TABLE IF NOT EXISTS direction (
            	id SERIAL,
            	step Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        await _dataAccess.ExecuteStatement(sqlRecipe, new { });
        await _dataAccess.ExecuteStatement(sqlIngredient, new { });
        await _dataAccess.ExecuteStatement(sqlDirection, new { });
    }
}