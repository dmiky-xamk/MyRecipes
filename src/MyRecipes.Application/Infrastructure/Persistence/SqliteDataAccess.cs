using Dapper;
using Microsoft.Extensions.Configuration;
using MyRecipes.Application.Entities;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;
using Npgsql;
using System.Data;
using System.Data.SQLite;

namespace MyRecipes.Infrastructure.Persistence;

/// <summary>
/// Handles the direct contact with the Sqlite database.
/// </summary>
internal class SqliteDataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public SqliteDataAccess(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("Default");
    }

    /// <summary>
    /// Queries the database for recipes along with their ingredients.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlStatement"></param>
    /// <param name="parameters"></param>
    /// <returns>An IEnumerable of the recipes that matched the parameters.</returns>
    public async Task<IEnumerable<RecipeEntity>> QueryRecipes<T>(string sqlStatement, T parameters)
    {
        // TODO: Create RecipeDataAccess?
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            // All the recipes to return along with their ingredients will be added here.
            var recipeDictionary = new Dictionary<string, RecipeEntity>();

            var recipes = await connection.QueryAsync<RecipeEntity, IngredientEntity, DirectionEntity, RecipeEntity>(sqlStatement, (recipe, ingredient, direction) =>
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
                if (!recipe.Ingredients.Any(i => i.Name == ingredient.Name))
                {
                    recipe.Ingredients.Add(ingredient);
                }

                // Database doesn't allow empty or whitespace direction steps, but when
                // mapping, Dapper creates 'DirectionEntity' which has a empty string as default value for 'Step'.
                // Rather than returning list with empty steps, we return an empty list.
                if (direction.Step.Length != 0 && !recipe.Directions.Any(d => d.Step == direction.Step))
                {
                    recipe.Directions.Add(direction);
                }

                return recipe;
            },
            splitOn: "IngredientId, DirectionId",
            param: parameters);

            return recipeDictionary.Values;
        }
    }

    public async Task<List<T>> QueryData<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            List<T> rows = (await connection.QueryAsync<T>(sqlStatement, parameters)).ToList();

            return rows;
        }
    }

    public async Task<T?> QueryDataSingle<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sqlStatement, parameters);
        }
    }

    /// <summary>
    /// Execute a SQL statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlStatement"></param>
    /// <param name="parameters"></param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> ExecuteStatement<T>(string sqlStatement, T parameters)
    {
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sqlStatement, parameters);
        }
    }

    public async Task<bool> ExecuteScalar<T>(string sqlStatement, T parameters)
    {
        using (IDbConnection connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.ExecuteScalarAsync<bool>(sqlStatement, parameters);
        }
    }
}