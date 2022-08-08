using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Infrastructure.Persistence;

public class Crud : ICrud
{
    private readonly IDataAccess _db;

    public Crud(IDataAccess db)
    {
       _db = db;
    }

    public async Task<IEnumerable<RecipeEntity>> GetFullRecipesAsync(string userId)
    {
        string sql = "SELECT r.Id, r.UserId, r.Name, r.Description, r.Image, i.Id, i.RecipeId, i.Name, i.Unit, i.Amount" +
                    " FROM Recipe r" +
                    " INNER JOIN Ingredient i on i.RecipeId = r.Id" +
                    " WHERE r.UserId = @UserId";

        return await _db.QueryRecipes<dynamic>(sql, new { UserId = userId });
    }

    public async Task<RecipeEntity?> GetFullRecipeAsync(string recipeId, string userId)
    {
        string sql = "SELECT r.Id, r.UserId, r.Name, r.Description, r.Image, i.Id, i.RecipeId, i.Name, i.Unit, i.Amount" +
                    " FROM Recipe r" +
                    " INNER JOIN Ingredient i on i.RecipeId = r.Id" +
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

    public async Task<int> UpdateRecipeAsync(RecipeEntity recipe)
    {
        string sql = "UPDATE Recipe SET Name = @Name, Description = @Description, Image = @Image" +
                    " WHERE Id = @Id";

        return await _db.ExecuteStatement(sql, new { recipe.Id, recipe.Name, recipe.Description, recipe.Image });
    }
    
    public async Task UpdateIngredientsAsync(IEnumerable<IngredientEntity> ingredients)
    {
        string sql = "DELETE FROM Ingredient" +
                    " WHERE RecipeId = @RecipeId";

        await _db.ExecuteStatement(sql, new { ingredients.First().RecipeId });

        await CreateIngredientsAsync(ingredients);
    }

    public async Task<int> DeleteRecipeAsync(string id)
    {
        // Delete all the ingredients as well.
        string sql = "PRAGMA foreign_keys=ON; DELETE FROM Recipe WHERE Id = @Id";

        return await _db.ExecuteStatement(sql, new { Id = id });
    }

    // TODO: Are these required anymore?
    //public async Task<List<RecipeEntity>> GetRecipesAsync()
    //{
    //    string sql = "SELECT * FROM Recipe;";

    //    return await _db.QueryData<RecipeEntity, dynamic>(sql, new { });
    //}

    //public async Task<RecipeEntity?> GetRecipeAsync(string id)
    //{
    //    // Liitä ainesosat samalla?
    //    string sql = "SELECT * FROM Recipe WHERE Id = @Id;";

    //    return await _db.QueryDataSingle<RecipeEntity, dynamic>(sql, new { Id = id });
    //}

    //public async Task<List<IngredientEntity>> GetIngredientsAsync(string recipeId)
    //{
    //    string sql = "SELECT i.*" +
    //                " FROM Ingredient i" +
    //                " INNER JOIN Recipe r" +
    //                " ON r.Id = i.RecipeId" +
    //                " WHERE i.RecipeId = @RecipeId";

    //    return await _db.QueryData<IngredientEntity, dynamic>(sql, new { RecipeId = recipeId });
    //}
}