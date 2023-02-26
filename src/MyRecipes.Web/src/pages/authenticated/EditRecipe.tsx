import { Typography } from "@mui/material";
import { Fragment } from "react";
import { useParams } from "react-router-dom";
import { useRecipe, useUpdateRecipe } from "../../features/recipes/recipe";
import RecipeForm from "../../features/recipes/RecipeForm";
import FullPageSpinner from "../../features/ui/loading/FullPageSpinner";
import PageContainer from "../../features/ui/main/PageContainer";

export default function EditRecipe() {
  // Typescript doesn't know that the id can't be undefined.
  // https://stackoverflow.com/questions/59085911/required-url-param-on-react-router-v5-with-typescript-can-be-undefined
  const { id } = useParams() as { id: string };

  // TODO: Handle 404 error boundary or not?
  const { recipe, isLoading, isError } = useRecipe(id);
  const mutate = useUpdateRecipe();

  if (isError) {
    return (
      <PageContainer center={true}>
        <Typography variant="h5">The recipe couldn't be found</Typography>
      </PageContainer>
    );
  }

  if (isLoading) {
    return (
      <Fragment>
        <FullPageSpinner text="Loading your recipe..." />
      </Fragment>
    );
  }

  return (
    <PageContainer sx={{ mt: 0, p: 0 }}>
      <RecipeForm mutate={mutate} recipe={recipe} />
    </PageContainer>
  );
}
