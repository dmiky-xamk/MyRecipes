using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRecipes.Application.Entities;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;
using MyRecipes.Infrastructure.Identity;

namespace MyRecipes.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IDbConnectionFactory dbConnectionFactory, IRecipeRepository recipeRepository)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _dbConnectionFactory = dbConnectionFactory;
        _recipeRepository = recipeRepository;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlite())
            {
                await _context.Database.MigrateAsync();
            }

            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    private async Task TrySeedAsync()
    {
        await CreateTestUser();
        await CreateTables();
        await CreateRecipesForTestUser();
    }
    
    private async Task CreateTestUser()
    {
        if (!_userManager.Users.Any())
        {
            ApplicationUser testUser = new()
            {
                UserName = "test@test.com",
                Email = "test@test.com"
            };

            if (_userManager.Users.All(u => u.Email != testUser.Email))
            {
                await _userManager.CreateAsync(testUser, "Test123");
            }

            await _context.SaveChangesAsync();
        }
    }
    
    private async Task CreateTables()
    {
        var queryRecipeTable = """
                CREATE TABLE IF NOT EXISTS recipe ( 
            	id Text NOT NULL,
            	name Text NOT NULL,
            	description Text NOT NULL DEFAULT '',
            	image Text NOT NULL DEFAULT '',
            	user_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES "AspNetUsers"("Id"));
            """;

        var queryIngredientTable = """
                CREATE TABLE IF NOT EXISTS ingredient ( 
            	id SERIAL,
            	name Text NOT NULL,
            	unit Text NOT NULL DEFAULT '',
            	amount Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        var queryDirectionTable = """
                CREATE TABLE IF NOT EXISTS direction ( 
            	id SERIAL,
            	step Text NOT NULL DEFAULT '',
            	recipe_id Text NOT NULL,
            	PRIMARY KEY (id),
                CONSTRAINT fk_recipe FOREIGN KEY(recipe_id) REFERENCES recipe(id) ON DELETE CASCADE);
            """;

        using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            await connection.ExecuteAsync(queryRecipeTable);
            await connection.ExecuteAsync(queryIngredientTable);
            await connection.ExecuteAsync(queryDirectionTable);
        }
    }
    
    private async Task CreateRecipesForTestUser()
    {
        var userId = _userManager.Users.First(u => u.Email == "test@test.com").Id;
        var recipes = await _recipeRepository.GetRecipesAsync(userId);

        if (!recipes.Any())
        {
            var recipe = new RecipeEntity()
            {
                Id = "123",
                Name = "Kaurapuuro",
                Description = "Helppo ja nopea",
                UserId = userId,
                Ingredients = new List<IngredientEntity>()
                {
                    new("123", "Vettä", "dl", "2 1/2"),
                    new("123", "Kaurahiutaleita", "dl", "1"),
                    new("123", "Suolaa", "tl", "2"),
                },
                Directions = new List<DirectionEntity>()
                {
                    new("123", "Kiehauta vesi"),
                    new("123", "Lisää kaurahiutaleet"),
                    new("123", "Anna kiehua hitaasti kunnes mieleistä")
                }
            };

            await _recipeRepository.CreateRecipeAsync(recipe);
        }
    }
}