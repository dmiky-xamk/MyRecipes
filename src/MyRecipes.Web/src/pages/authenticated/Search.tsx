import { Check } from "@mui/icons-material";
import {
  Autocomplete,
  Card,
  CardContent,
  Divider,
  List,
  ListItem,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import { Ingredient, Recipe, useRecipes } from "../../features/recipes/recipe";
import PageContainer from "../../features/ui/main/PageContainer";

interface MatchedRecipe {
  recipe: Recipe;
  matchingIngredients: Ingredient[];
  nonMatchingIngredients: Ingredient[];
  totalIngredientsCount: number;
}

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
    <PageContainer>
      {/* <Stack justifyContent="end"> */}
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
          <TextField {...params} label="Ingredient" disabled={isLoading} />
        )}
      />
      <List>
        {matchedRecipes.map((matched: MatchedRecipe) => {
          return (
            <ListItem key={matched.recipe.id}>
              <Card sx={{ flex: 1 }}>
                <CardContent>
                  <Stack direction="row" justifyContent="space-between">
                    <Typography variant="h6" gutterBottom>
                      {matched.recipe.name}
                    </Typography>
                    <Typography>{`${matched.matchingIngredients.length} / ${matched.totalIngredientsCount}`}</Typography>
                  </Stack>
                  <Stack direction="row" gap={2} flexWrap="wrap">
                    {matched.matchingIngredients.map((ing) => {
                      return <Typography key={ing.name}>{ing.name}</Typography>;
                    })}
                  </Stack>
                  <Divider component="li" sx={{ my: 1 }} />
                  <Stack direction="row" gap={2} flexWrap="wrap">
                    {matched.nonMatchingIngredients.map((ing) => {
                      return (
                        <Typography
                          key={ing.name}
                          variant="body2"
                          color="GrayText"
                        >
                          {ing.name}
                        </Typography>
                      );
                    })}
                  </Stack>
                </CardContent>
              </Card>
            </ListItem>
          );
        })}
      </List>
      {/* </Stack> */}
    </PageContainer>
  );
}
