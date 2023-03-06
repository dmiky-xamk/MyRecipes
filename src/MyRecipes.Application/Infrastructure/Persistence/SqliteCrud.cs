using MyRecipes.Application.Entities;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Infrastructure.Persistence;

// Write the appropriate SQL statements here and pass them to *DataAccess for execution.
public class SqliteCrud : ICrud
{
    private readonly IDataAccess _db;

    public SqliteCrud(IDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<RecipeEntity>> GetFullRecipesAsync(string userId)
    {
        string sql = "SELECT r.Id, r.UserId, r.Name, r.Description, r.Image, i.Id AS IngredientId, i.RecipeId, i.Name, i.Unit, i.Amount, d.Id AS DirectionId, d.Step" +
                    " FROM Recipe r" +
                    " INNER JOIN Ingredient i on i.RecipeId = r.Id" +
                    " INNER JOIN Direction d on d.RecipeId = r.Id" +
                    " WHERE r.UserId = @UserId";

        return await _db.QueryRecipes<dynamic>(sql, new { UserId = userId });
    }

    public async Task<RecipeEntity?> GetFullRecipeAsync(string recipeId, string userId)
    {
        string sql = "SELECT r.Id, r.UserId, r.Name, r.Description, r.Image, i.Id, i.RecipeId, i.Name, i.Unit, i.Amount, d.Step" +
                    " FROM Recipe r" +
                    " INNER JOIN Ingredient i on i.RecipeId = r.Id" +
                    " INNER JOIN Direction d on d.RecipeId = r.Id" +
                    " WHERE i.RecipeId = @RecipeId AND r.UserId = @UserId";

        return (await _db.QueryRecipes<dynamic>(sql, new { RecipeId = recipeId, UserId = userId }))
            .FirstOrDefault();
    }

    public async Task<int> CreateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "INSERT INTO Recipe (Id, UserId, Name, Description, Image)" +
                    " VALUES (@Id, @UserId, @Name, @Description, @Image);";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task CreateIngredientsAsync(IEnumerable<IngredientEntity> ingredients)
    {
        string sql = "INSERT INTO Ingredient (RecipeId, Name, Unit, Amount)" +
                    " VALUES (@RecipeId, @Name, @Unit, @Amount);";

        await _db.ExecuteStatement(sql, ingredients);
    }

    public async Task CreateDirectionsAsync(IEnumerable<DirectionEntity> directions)
    {
        string sql = "INSERT INTO Direction (RecipeId, Step)" +
                    " VALUES (@RecipeId, @Step);";

        await _db.ExecuteStatement(sql, directions);
    }

    public async Task<int> UpdateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "UPDATE Recipe SET Name = @Name, Description = @Description, Image = @Image" +
                    " WHERE Id = @Id AND UserId = @UserId";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
    }

    public async Task UpdateIngredientsAsync(IEnumerable<IngredientEntity> ingredients, string recipeId)
    {
        string sql = "DELETE FROM Ingredient" +
                    " WHERE RecipeId = @RecipeId";

        await _db.ExecuteStatement(sql, new { RecipeId = recipeId });

        await CreateIngredientsAsync(ingredients);
    }

    public async Task UpdateDirectionsAsync(IEnumerable<DirectionEntity> directions, string recipeId)
    {
        string sql = "DELETE FROM Direction" +
                    " WHERE RecipeId = @RecipeId";

        await _db.ExecuteStatement(sql, new { recipeId });

        await CreateDirectionsAsync(directions);
    }

    public async Task<int> DeleteRecipeAsync(string id, string userId)
    {
        // Delete all the ingredients as well.
        string sql = "PRAGMA foreign_keys=ON; " +
                    " DELETE FROM Recipe " +
                    " WHERE Id = @Id AND UserId = @UserId";

        return await _db.ExecuteStatement(sql, new { Id = id, UserId = userId });
    }
}