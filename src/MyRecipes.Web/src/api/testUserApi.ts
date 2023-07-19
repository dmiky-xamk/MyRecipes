import { Recipe } from "../features/recipes/recipe";
import testUserRecipes from "../assets/testUserRecipes.json";

const testRecipes = testUserRecipes as Recipe[];

export const TestUserRecipes = {
  list: (): Promise<Recipe[]> => Promise.resolve(testRecipes),
  details: (id: string): Promise<Recipe> =>
    Promise.resolve(testRecipes.find((r) => r.id === id)) as Promise<Recipe>,
  update: (id: string, recipe: Recipe) =>
    Promise.resolve(updateRecipe(id, recipe)),
  create: (recipe: Recipe) => Promise.resolve(createRecipe(recipe)),
  delete: (id: string) => Promise.resolve(deleteRecipe(id)),
};

const updateRecipe = (id: string, recipe: Recipe) => {
  const index = testRecipes.findIndex((r) => r.id === id);
  recipe.image = testRecipes[index].image;
  testRecipes[index] = recipe;

  return recipe;
};

const createRecipe = (recipe: Recipe) => {
  recipe.id = (testRecipes.length + 1).toString();
  testRecipes.push(recipe);

  return recipe;
};

const deleteRecipe = (id: string) => {
  console.log("Delete recipe:", id);

  const index = testRecipes.findIndex((r) => r.id === id);
  testRecipes.splice(index, 1);
};
