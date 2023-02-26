import { Autocomplete, Box, List, TextField, Typography } from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import {
  Ingredient,
  MatchedRecipe,
  useRecipes,
} from "../../features/recipes/recipe";
import { RecipeEditTextField } from "../../features/recipes/RecipeEditTextField";
import RecipeSearchCard from "../../features/recipes/RecipeSearchCard";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Search() {
  const { recipes, isLoading } = useRecipes();
  const [matchedRecipes, setMatchedRecipes] = useState<MatchedRecipe[]>([]);
  const [autoCompleteValues, setAutoCompleteValues] = useState<string[]>([]);

  // TODO: Memo?
  const ingredientNames = recipes.flatMap((recipe) =>
    recipe.ingredients.map((ing) => {
      return ing.name;
    })
  );

  // Remove any duplicate ingredient names.
  const uniqueIngredientNames: string[] = Array.from(new Set(ingredientNames));

  // Find the recipes that have some of the ingredients
  // that the user passed in through the 'Autocomplete'.
  const matchRecipesWithIngredients = useCallback(
    (ingredients: string[]) => {
      const matched: MatchedRecipe[] = recipes
        .map((recipe) => {
          // Finds the ingredients that match and don't match the user's query.
          const [matchingIngredients, nonMatchingIngredients] =
            recipe.ingredients.reduce(
              ([matched, nonMatched], ingredient) => {
                return ingredients.includes(ingredient.name)
                  ? [[...matched, ingredient], nonMatched]
                  : [matched, [...nonMatched, ingredient]];
              },
              [[], []] as [Ingredient[], Ingredient[]]
            );
          return {
            recipe,
            matchingIngredients,
            nonMatchingIngredients,
            totalIngredientsCount: recipe.ingredients.length,
          };
        })
        .filter((match) => match.matchingIngredients.length > 0)
        .sort(
          (a, b) => b.matchingIngredients.length - a.matchingIngredients.length
        );

      setMatchedRecipes(matched);
    },
    [recipes]
  );

  useEffect(() => {
    matchRecipesWithIngredients(autoCompleteValues);
  }, [matchRecipesWithIngredients, autoCompleteValues]);

  return (
    <PageContainer fullHeight>
      <Autocomplete
        disablePortal
        filterSelectedOptions
        multiple
        value={autoCompleteValues}
        onChange={(_: any, newValue: string[]) => {
          setAutoCompleteValues(newValue);
        }}
        id="ingredient-search"
        options={uniqueIngredientNames}
        renderInput={(params) => (
          <RecipeEditTextField
            {...params}
            label="Ingredients"
            disabled={isLoading}
          />
        )}
      />
      <List>
        {matchedRecipes.map((matched: MatchedRecipe) => {
          return (
            <RecipeSearchCard
              key={matched.recipe.id}
              recipe={matched.recipe}
              matchingIngredients={matched.matchingIngredients}
              nonMatchingIngredients={matched.nonMatchingIngredients}
              totalIngredientsCount={matched.totalIngredientsCount}
            />
          );
        })}
      </List>
    </PageContainer>
  );
}
