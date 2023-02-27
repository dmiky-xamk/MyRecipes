using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Infrastructure.Persistence;

public class PostgreSqlCrud : ICrud
{
    private readonly IDataAccess _db;

    public PostgreSqlCrud(IDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<RecipeEntity>> GetFullRecipesAsync(string userId)
    {
        string sql = """
                    SELECT r.id, r.user_id, r.name, r.description, r.image, i.id, i.recipe_id, i.name, i.unit, i.amount
                    FROM recipe r
                    inner join ingredient i ON i.recipe_id = r.id
                    WHERE r.user_id = @UserId
                    """;

        return await _db.QueryRecipes<dynamic>(sql, new { UserId = userId });
    }

    public async Task<RecipeEntity?> GetFullRecipeAsync(string recipeId, string userId)
    {
        string sql = "SELECT r.id, r.user_id, r.name, r.description, r.image, i.id, i.recipe_id, i.name, i.unit, i.amount" +
                    " FROM recipe r" +
                    " INNER JOIN ingredient i on i.recipe_id = r.id" +
                    " WHERE i.recipe_id = @RecipeId AND r.user_id = @UserId";

        return (await _db.QueryRecipes<dynamic>(sql, new { RecipeId = recipeId, UserId = userId }))
            .FirstOrDefault();
    }

    public async Task<int> CreateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "INSERT INTO recipe (id, user_id, name, description, image)" +
                    " VALUES (@Id, @UserId, @Name, @Description, @Image);";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task CreateIngredientsAsync(IEnumerable<IngredientEntity> ingredients)
    {
        string sql = "INSERT INTO ingredient (recipe_id, name, unit, amount)" +
                    " VALUES (@RecipeId, @Name, @Unit, @Amount);";

        await _db.ExecuteStatement(sql, ingredients);
    }

    public async Task<int> UpdateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "UPDATE Recipe SET name = @Name, description = @Description, image = @Image" +
                    " WHERE id = @Id AND user_id = @UserId";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task UpdateIngredientsAsync(IEnumerable<IngredientEntity> ingredients)
    {
        string sql = "DELETE FROM ingredient" +
                    " WHERE recipe_id = @RecipeId";

        await _db.ExecuteStatement(sql, new { ingredients.First().RecipeId });

        await CreateIngredientsAsync(ingredients);
    }

    public async Task<int> DeleteRecipeAsync(string id, string userId)
    {
        // Delete all the ingredients as well.
        string sql = " DELETE FROM recipe " +
                     " WHERE id = @Id AND user_id = @UserId";

        return await _db.ExecuteStatement(sql, new { Id = id, UserId = userId });
    }
}