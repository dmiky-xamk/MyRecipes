import {
  QueryClient,
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import agent, { ApiErrorResponse } from "../../api/agent";

export interface Recipe {
  id: string;
  name: string;
  description: string;
  image: string;
  ingredients: Ingredient[];
  directions: Direction[];
}

export interface Direction {
  step: string;
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
  fieldsArray: Ingredient[];
  directionsArray: Direction[];
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
        queryClient.setQueryData(["recipes", recipe.id], recipe);
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
    queryKey: ["recipes", recipeId],
    queryFn: () => agent.Recipes.details(recipeId),
    retry: 0,
    useErrorBoundary: false, // ASETA TÄMÄ!!
    // onError: (err) => {
    //   console.log("Error:", err);
    // },

    // onSuccess: (recipes: Recipe[]) => {
    //   for (const recipe of recipes) {
    //     queryClient.setQueryData(["recipes", recipe.id], recipe);
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
        //  queryClient.invalidateQueries(["recipes", recipe.id])
      },
    }
  );
};

// TODO: Optimistic update?
const useUpdateRecipe = () => {
  const queryClient = useQueryClient();

  return useMutation<
    Recipe,
    RecipeErrorResponse,
    Recipe,
    { previousRecipe: Recipe | undefined; newRecipe: Recipe }
  >((recipe: Recipe) => agent.Recipes.update(recipe.id, recipe), {
    onMutate: async (newRecipe: Recipe) => {
      console.log("Begin optimistic update");
      // Cancel any outgoing refetches
      // (so they don't overwrite our optimistic update)
      await queryClient.cancelQueries({ queryKey: ["recipes", newRecipe.id] });

      // Snapshot the previous value
      const previousRecipe = queryClient.getQueryData<Recipe>([
        "recipes",
        newRecipe.id,
      ]);

      console.log("Previous recipe:", previousRecipe);
      console.log("Updated recipe:", newRecipe);

      // Optimistically update to the new value
      queryClient.setQueryData(["recipes", newRecipe.id], newRecipe);
      queryClient.setQueryData(["recipes"], (old: any) => {
        return old.map((item: Recipe) => {
          return item.id === newRecipe.id ? { ...item, ...newRecipe } : item;
        });
      });

      // Return a context object with the snapshotted value
      return { previousRecipe, newRecipe };
    },
    onError: (err, newRecipe, context) => {
      console.log("onError optimistic update");
      queryClient.setQueryData(
        ["recipes", context?.newRecipe.id],
        context?.previousRecipe
      );
    },
    onSettled: (recipe: Recipe | undefined) => {
      console.log("onSettled optimistic update");
      queryClient.invalidateQueries(["recipes", recipe?.id]);
    },
  });
};

// React query optimistically updates the cache

// async function onUpdateMutation(recipe: Recipe, queryClient: QueryClient) {
//   // Cancel any outgoing refetches
//   // (so they don't overwrite our optimistic update)
//   await queryClient.cancelQueries({ queryKey: ["recipes"] });

//   // Snapshot the previous value
//   const previousTodos = queryClient.getQueryData(["recipes"]);

//   // Optimistically update to the new value
//   queryClient.setQueryData(["recipes"], (old: any) => [...old, newTodo]);

//   // Return a context object with the snapshotted value
//   return { previousTodos };

//   return () => queryClient.setQueryData(["recipes"], previousItems);
// }

export { useRecipe, useRecipes, useCreateRecipe, useUpdateRecipe };
