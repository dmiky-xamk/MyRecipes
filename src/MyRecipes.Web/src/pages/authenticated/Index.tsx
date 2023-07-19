import { Grid } from "@mui/material";
import { Link } from "react-router-dom";
import CreateRecipeFab from "../../features/recipes/CreateRecipeFab";
import NoRecipes from "../../features/recipes/NoRecipes";
import { Recipe, useRecipes } from "../../features/recipes/recipe";
import RecipeCard from "../../features/recipes/RecipeCard";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Index() {
  // This will use the cached recipes.
  const { recipes: userRecipes } = useRecipes();

  const recipes = userRecipes.map((recipe: Recipe) => {
    return (
      <Grid item key={recipe.id} display="flex" xs={12} sm={6} md={4}>
        <Link
          to={`recipe/${recipe.id}`}
          style={{ textDecoration: "none", display: "flex", flex: "1" }}
        >
          <RecipeCard recipe={recipe} />
        </Link>
      </Grid>
    );
  });

  if (!recipes.length) {
    return (
      <PageContainer center={true}>
        <NoRecipes />
        <CreateRecipeFab />
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <Grid container spacing={2} pb={2}>
        {recipes}
      </Grid>
      <CreateRecipeFab />
    </PageContainer>
  );
}
