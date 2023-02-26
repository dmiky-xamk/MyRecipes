import { Add } from "@mui/icons-material";
import { Fab, List, ListItem, ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import NoRecipes from "../../features/recipes/NoRecipes";
import { Recipe, useRecipes } from "../../features/recipes/recipe";
import RecipeCard from "../../features/recipes/RecipeCard";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Index() {
  const navigate = useNavigate();

  // This will use the cached recipes.
  const { recipes: userRecipes } = useRecipes();

  const recipes = userRecipes.map((recipe: Recipe) => {
    return (
      <ListItem key={recipe.id}>
        <ListItemButton
          disableGutters
          dense
          onClick={() => navigate(`recipe/${recipe.id}`)}
        >
          <RecipeCard recipe={recipe} />
        </ListItemButton>
      </ListItem>
    );
  });

  if (!recipes.length) {
    return (
      <PageContainer center={true}>
        <NoRecipes />
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <List>{recipes}</List>
      <Fab
        color="primary"
        aria-label="Create a new recipe"
        sx={{
          position: "fixed",
          right: (theme) => theme.spacing(1),
          bottom: (theme) => `calc(56px + ${theme.spacing(1)})`,
        }}
        onClick={() => navigate("recipe/create")}
      >
        <Add />
      </Fab>
    </PageContainer>
  );
}
