import { AddShoppingCart, Edit } from "@mui/icons-material";
import { Box, IconButton, Stack, Typography } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { useRecipe } from "../../features/recipes/recipe";
import RecipePlaceholderImage from "../../features/recipes/RecipePlaceholderImage";
import FullPageSpinner from "../../features/ui/loading/FullPageSpinner";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Recipe() {
  // Typescript doesn't know that the id can't be undefined.
  // https://stackoverflow.com/questions/59085911/required-url-param-on-react-router-v5-with-typescript-can-be-undefined
  const { id } = useParams() as { id: string };
  const navigate = useNavigate();

  // TODO: Handle 404 error boundary or not?
  const { recipe, isLoading, isError } = useRecipe(id);

  if (isError) {
    return <PageContainer center={true}>Error</PageContainer>;
  }

  if (isLoading) {
    return (
      <PageContainer center={true}>
        <FullPageSpinner text="Loading your recipe..." />
      </PageContainer>
    );
  }

  // TODO: Different key?
  const ingredients = recipe!.ingredients.map((ingredient) => {
    return (
      <Typography key={ingredient.name}>
        {ingredient.amount} {ingredient.unit} {ingredient.name}
      </Typography>
    );
  });

  return (
    <PageContainer sx={{ mt: 0, p: 0 }}>
      {recipe!.image || <RecipePlaceholderImage />}
      <Stack padding={2} direction="column" gap={2}>
        <Box>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Typography variant="h5" gutterBottom>
              {recipe!.name}
            </Typography>
            <IconButton
              aria-label="Edit recipe"
              onClick={() => navigate(`edit`)}
            >
              <Edit />
            </IconButton>
          </Stack>
          <Typography color="GrayText">{recipe!.description}</Typography>
        </Box>
        <Box>
          <Stack direction="row" alignItems="center" gap={1} mb={0.1}>
            <Typography variant="h6">Ingredients</Typography>
            <IconButton aria-label="Add ingredients to cart">
              <AddShoppingCart />
            </IconButton>
          </Stack>
          <Stack gap={0.2}>{ingredients}</Stack>
        </Box>
      </Stack>
    </PageContainer>
  );
}
