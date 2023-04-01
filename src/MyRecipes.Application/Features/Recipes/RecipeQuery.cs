namespace MyRecipes.Application.Features.Recipes;

/// <summary>
/// Contains all the queries for the <see cref="RecipeRepository"/>.
/// </summary>
public static class RecipeQuery
{
    public const string GetRecipesQuery = """
                    SELECT r.id, r.user_id, r.name, r.description, r.image,
                           i.id AS IngredientId, i.recipe_id, i.name, i.unit, i.amount,
                           d.id AS DirectionId, d.step
                    FROM recipe r
                    INNER JOIN ingredient i on i.recipe_id = r.id
                    LEFT OUTER JOIN direction d ON d.recipe_id = r.id
                    WHERE r.user_id = @UserId
                    ORDER by r.id, i.id, d.id;
                    """;
    
    public const string GetRecipeQuery = """
                    SELECT r.id, r.user_id, r.name, r.description, r.image,
                           i.id AS IngredientId, i.recipe_id, i.name, i.unit, i.amount,
                           d.id AS DirectionId, d.step
                    FROM recipe r
                    INNER JOIN ingredient i on i.recipe_id = r.id
                    LEFT OUTER JOIN direction d ON d.recipe_id = r.id
                    WHERE i.recipe_id = @RecipeId AND r.user_id = @UserId
                    ORDER by r.id, i.id, d.id;
                    """;
    
    public const string CreateRecipeQuery = """
                    INSERT INTO recipe (id, user_id, name, description, image)
                    VALUES (@Id, @UserId, @Name, @Description, @Image);
                    """;

    public const string UpdateRecipeQuery = """
                    UPDATE Recipe SET name = @Name, description = @Description, image = @Image
                    WHERE id = @Id AND user_id = @UserId;
                    """;

    public const string DeleteRecipeQuery = """
                    DELETE FROM recipe
                    WHERE id = @Id AND user_id = @UserId;
                    """;

    public const string CreateIngredientsQuery = """
                    INSERT INTO ingredient (recipe_id, name, unit, amount)
                    VALUES (@RecipeId, @Name, @Unit, @Amount);
                    """;
    
    public const string DeleteIngredientsQuery = """
                    DELETE FROM ingredient
                    WHERE recipe_id = @RecipeId;
                    """;

    public const string CreateDirectionsQuery = """
                    INSERT INTO direction (recipe_id, step)
                    VALUES (@RecipeId, @Step);
                    """;
    
    public const string DeleteDirectionsQuery = """
                    DELETE FROM direction
                    WHERE recipe_id = @RecipeId;
                    """;
    
    public const string CheckIfRecipeExistsQuery = """
            SELECT CAST(CASE WHEN EXISTS 
            (
                SELECT 1 FROM recipe
                WHERE id = @Id AND user_id = @UserId
            )
            THEN 1 ELSE 0 END as BIT);
            """;
}