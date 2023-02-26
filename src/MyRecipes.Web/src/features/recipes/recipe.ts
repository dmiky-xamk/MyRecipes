import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../../api/agent";

export interface Recipe {
  id: string;
  name: string;
  description: string;
  image: string;
  ingredients: Ingredient[];
}

export interface Ingredient {
  name: string;
  unit: string;
  amount: string;
}

export interface MatchedRecipe {
  recipe: Recipe;
  matchingIngredients: Ingredient[];
  nonMatchingIngredients: Ingredient[];
  totalIngredientsCount: number;
}

export interface FormFields {
  recipeName: string;
  recipeDescription: string;
  recipeDirections: string;
  fieldsArray: {
    name: string;
    amount: string;
    unit: string;
  }[];
}

interface ApiErrorResponse {
  status: number;
  title: string;
  traceId: string;
  type: string;
}

export interface RecipeErrorResponse extends ApiErrorResponse {
  errors?: {
    Name?: [];
    Ingredients?: [];
  };
}

// Fetch the user's recipes and update the cache.
// Also update the cache for the full recipe information.
const useRecipes = () => {
  const queryClient = useQueryClient();

  const { data: recipes, isLoading } = useQuery({
    queryKey: ["recipes"],
    queryFn: () => agent.Recipes.list(),

    onSuccess: (recipes: Recipe[]) => {
      for (const recipe of recipes) {
        queryClient.setQueryData(["recipe", recipe.id], recipe);
      }
    },
  });

  return { recipes: recipes || [], isLoading };
};

const useRecipe = (recipeId: string) => {
  // const queryClient = useQueryClient();

  const {
    data: recipe,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["recipe", recipeId],
    queryFn: () => agent.Recipes.details(recipeId),
    retry: 0,
    useErrorBoundary: false, // ASETA TÄMÄ!!
    // onError: (err) => {
    //   console.log("Error:", err);
    // },

    // onSuccess: (recipes: Recipe[]) => {
    //   for (const recipe of recipes) {
    //     queryClient.setQueryData(["recipe", recipe.id], recipe);
    //   }
    // },
  });

  return { recipe, isLoading, isError, error };
};

const useCreateRecipe = () => {
  return useMutation<any, RecipeErrorResponse, Recipe, void>(
    (recipe: Recipe) => agent.Recipes.create(recipe),
    {
      onSuccess: (newRecipe) => {
        console.log("Create recipe:", newRecipe);
        //  queryClient.invalidateQueries(["recipe", recipe.id])
      },
    }
  );
};

// TODO: Optimistic update?
const useUpdateRecipe = () => {
  const queryClient = useQueryClient();

  return useMutation<any, RecipeErrorResponse, Recipe, void>(
    (recipe: Recipe) => agent.Recipes.update(recipe.id, recipe),
    {
      onSuccess: (newRecipe) => {
        console.log("Update recipe:", newRecipe);
        //  queryClient.invalidateQueries(["recipe", recipe.id])
      },
    }
  );
};

export { useRecipe, useRecipes, useCreateRecipe, useUpdateRecipe };
