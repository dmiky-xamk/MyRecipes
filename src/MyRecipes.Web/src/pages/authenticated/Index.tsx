import { List, ListItem, ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import CreateRecipeFab from "../../features/recipes/CreateRecipeFab";
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
        <CreateRecipeFab />
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <List>{recipes}</List>
      <CreateRecipeFab />
    </PageContainer>
  );
}
