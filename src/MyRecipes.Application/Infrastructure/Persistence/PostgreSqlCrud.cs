using MyRecipes.Application.Entities;
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
                    SELECT r.id, r.user_id, r.name, r.description, r.image, i.id AS IngredientId, i.recipe_id, i.name, i.unit, i.amount, d.id AS DirectionId, d.step
                    FROM recipe r
                    INNER JOIN ingredient i on i.recipe_id = r.id
                    LEFT OUTER JOIN direction d ON d.recipe_id = r.id
                    WHERE r.user_id = @UserId
                    ORDER by r.id, i.id, d.id;
                    """;

        return await _db.QueryRecipes<dynamic>(sql, new { UserId = userId });
    }

    public async Task<RecipeEntity?> GetFullRecipeAsync(string recipeId, string userId)
    {
        string sql = "SELECT r.id, r.user_id, r.name, r.description, r.image, i.id AS IngredientId, i.recipe_id, i.name, i.unit, i.amount, d.id AS DirectionId, d.step" +
                    " FROM recipe r" +
                    " INNER JOIN ingredient i on i.recipe_id = r.id" +
                    " LEFT OUTER JOIN direction d ON d.recipe_id = r.id" +
                    " WHERE i.recipe_id = @RecipeId AND r.user_id = @UserId" +
                    " ORDER by r.id, i.id, d.id;";

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

    public async Task CreateDirectionsAsync(IEnumerable<DirectionEntity> directions)
    {
        string sql = "INSERT INTO direction (recipe_id, step)" +
                    " VALUES (@RecipeId, @Step);";

        await _db.ExecuteStatement(sql, directions);
    }

    public async Task<int> UpdateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "UPDATE Recipe SET name = @Name, description = @Description, image = @Image" +
                    " WHERE id = @Id AND user_id = @UserId";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task UpdateIngredientsAsync(IEnumerable<IngredientEntity> ingredients, string recipeId)
    {
        string sql = "DELETE FROM ingredient" +
                    " WHERE recipe_id = @RecipeId";

        await _db.ExecuteStatement(sql, new { RecipeId = recipeId });

        await CreateIngredientsAsync(ingredients);
    }

    public async Task UpdateDirectionsAsync(IEnumerable<DirectionEntity> directions, string recipeId)
    {
        string sql = "DELETE FROM direction" +
                    " WHERE recipe_id = @RecipeId";

        await _db.ExecuteStatement(sql, new { RecipeId = recipeId });

        await CreateDirectionsAsync(directions);
    }

    public async Task<int> DeleteRecipeAsync(string id, string userId)
    {
        // Delete all the ingredients as well.
        string sql = " DELETE FROM recipe " +
                     " WHERE id = @Id AND user_id = @UserId";

        return await _db.ExecuteStatement(sql, new { Id = id, UserId = userId });
    }
}