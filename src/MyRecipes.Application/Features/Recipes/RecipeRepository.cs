using Dapper;
using MyRecipes.Application.Entities;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Features.Recipes;

/// <summary>
/// Repository for <see cref="RecipeEntity"/> related operations.
/// </summary>
public class RecipeRepository : IRecipeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RecipeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Gets all recipes for the given user id.
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>All the recipes that the user has.</returns>
    public async Task<IEnumerable<RecipeEntity>> GetRecipesAsync(string userId)
    {
        var param = new DynamicParameters(new { UserId = userId });
        
        return await QueryFullRecipesAsync(RecipeQuery.GetRecipesQuery, param);
    }
    
    /// <summary>
    /// Gets a recipe for the given id and user id.
    /// </summary>
    /// <param name="recipeId">Recipe id</param>
    /// <param name="userId">User id</param>
    /// <returns>The recipe if found; otherwise null.</returns>
    public async Task<RecipeEntity?> GetRecipeAsync(string recipeId, string userId)
    {
        var param = new DynamicParameters(new { RecipeId = recipeId, UserId = userId });
        
        return (await QueryFullRecipesAsync(RecipeQuery.GetRecipeQuery, param))
            .FirstOrDefault();
    }

    /// <summary>
    /// Creates a new recipe.
    /// </summary>
    /// <param name="recipe">The recipe to be created.</param>
    /// <returns>Whether the operation was successful or not.</returns>
    public async Task<bool> CreateRecipeAsync(RecipeEntity recipe)
    {
        return await UpsertFullRecipeAsync(recipe, RecipeQuery.CreateRecipeQuery);
    }
    
    /// <summary>
    /// Updates the recipe.
    /// </summary>
    /// <param name="recipe">The updated recipe.</param>
    /// <returns>Whether the operation was successful or not.</returns>
    public async Task<bool> UpdateRecipeAsync(RecipeEntity recipe)
    {
        return await UpsertFullRecipeAsync(recipe, RecipeQuery.UpdateRecipeQuery);
    }
    
    /// <summary>
    /// Deletes a recipe for the given id and user id.
    /// </summary>
    /// <param name="id">Recipe id</param>
    /// <param name="userId">User id</param>
    /// <returns>Whether the operation was successful or not.</returns>
    public async Task<bool> DeleteRecipeAsync(string id, string userId)
    {
        var param = new DynamicParameters(new { Id = id, UserId = userId });
        
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(RecipeQuery.DeleteRecipeQuery, param);

            return affectedRows == 1;
        }
    }

    /// <summary>
    /// Checks if a recipe exists for the given id and user id.
    /// </summary>
    /// <param name="id">Recipe id</param>
    /// <param name="userId">User id</param>
    /// <returns>Whether the recipe exists.</returns>
    public async Task<bool> CheckIfRecipeExistsAsync(string id, string userId)
    {
        var param = new DynamicParameters(new { Id = id, UserId = userId });

        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            return await connection.ExecuteScalarAsync<bool>(RecipeQuery.CheckIfRecipeExistsQuery, param);
        }
    }
    
    private async Task<IEnumerable<RecipeEntity>> QueryFullRecipesAsync(string sql, DynamicParameters param)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            // All the recipes to return along with their ingredients will be added here.
            var recipeDictionary = new Dictionary<string, RecipeEntity>();

            await connection.QueryAsync<RecipeEntity, IngredientEntity, DirectionEntity, RecipeEntity>(
                sql, (recipe, ingredient, direction) =>
                {
                    if (recipeDictionary.TryGetValue(recipe.Id, out RecipeEntity? existingRecipe))
                    {
                        recipe = existingRecipe;
                    }
                    else
                    {
                        recipeDictionary.Add(recipe.Id, recipe);
                    }

                    // Add ingredients that aren't in the list already.
                    if (recipe.Ingredients.All(i => i.Name != ingredient.Name))
                    {
                        recipe.Ingredients.Add(ingredient);
                    }

                    // Database doesn't allow empty or whitespace direction steps, but when
                    // mapping, Dapper creates 'DirectionEntity' which has a empty string as default value for 'Step'.
                    // Rather than returning list with empty steps, we return an empty list.
                    if (direction.Step.Length != 0 && recipe.Directions.All(d => d.Step != direction.Step))
                    {
                        recipe.Directions.Add(direction);
                    }

                    return recipe;
                },
                splitOn: "IngredientId, DirectionId",
                param: param);

            return recipeDictionary.Values;
        }
    }
    
    /// <summary>
    /// Creates or updates a recipe.
    /// </summary>
    /// <param name="recipe">The updated recipe.</param>
    /// <param name="recipeQuery">The query to be executed.</param>
    /// <returns>Whether operation was successful or not.</returns>
    private async Task<bool> UpsertFullRecipeAsync(RecipeEntity recipe, string recipeQuery)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync()) 
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var recipeParam = new DynamicParameters(new
                        { recipe.Id, recipe.UserId, recipe.Name, recipe.Description, recipe.Image });
                    await connection.ExecuteAsync(recipeQuery, recipeParam, transaction);

                    var deleteParam = new DynamicParameters(new { RecipeId = recipe.Id });
                    
                    await connection.ExecuteAsync(RecipeQuery.DeleteIngredientsQuery, deleteParam, transaction);
                    await connection.ExecuteAsync(RecipeQuery.CreateIngredientsQuery, recipe.Ingredients, transaction);

                    await connection.ExecuteAsync(RecipeQuery.DeleteDirectionsQuery, deleteParam, transaction);
                    await connection.ExecuteAsync(RecipeQuery.CreateDirectionsQuery, recipe.Directions, transaction);
                    
                    transaction.Commit();
                    
                    return true;
                }
                catch (Exception)
                {
                    // TODO: Log the exception.
                    
                    transaction.Rollback();

                    return false;
                }
            }
        }
    }
}