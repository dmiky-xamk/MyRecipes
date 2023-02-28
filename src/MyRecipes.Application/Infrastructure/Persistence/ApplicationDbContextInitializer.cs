using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;
using MyRecipes.Infrastructure.Identity;

namespace MyRecipes.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICrud _db;
    private readonly IDataAccess _dataAccess;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICrud db, IDataAccess dataAccess)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _db = db;
        _dataAccess = dataAccess;
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
            _logger.LogError(ex, "An error occured while initializing the database.");
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
            _logger.LogError(ex, "An error occured while seeding the database.");
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_userManager.Users.Any())
        {
            // Default user
            ApplicationUser adminUser = new()
            {
                UserName = "test@test.com",
                Email = "test@test.com"
            };

            if (_userManager.Users.All(u => u.Email != adminUser.Email))
            {
                await _userManager.CreateAsync(adminUser, "Test123");
            }

            await _context.SaveChangesAsync();
        }

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

        await _dataAccess.ExecuteStatement(sqlRecipe, new { });
        await _dataAccess.ExecuteStatement(sqlIngredient, new { });

        var userId = _userManager.Users.First().Id;
        var recipes = await _db.GetFullRecipesAsync(userId);

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
                    new IngredientEntity() { RecipeId = "123", Name = "Vettä", Amount = "2 1/2", Unit = "dl"  },
                    new IngredientEntity() { RecipeId = "123", Name = "Kaurahiutaleita", Amount = "1", Unit = "dl" },
                    new IngredientEntity() { RecipeId = "123", Name = "Suolaa", Amount = "2", Unit = "tl" },
                },
            };

            await _db.CreateRecipeAsync(recipe);
            await _db.CreateIngredientsAsync(recipe.Ingredients);
        }
    }
}