import { Delete, Edit } from "@mui/icons-material";
import {
  Box,
  Grid,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Stack,
  Typography,
} from "@mui/material";
import { ReactNode, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useDeleteRecipe, useRecipe } from "../../features/recipes/recipe";
import RecipeImage from "../../features/recipes/RecipeImage";
import FullPageSpinner from "../../features/ui/loading/FullPageSpinner";
import PageContainer from "../../features/ui/main/PageContainer";
import ConfirmationModal from "../../features/ui/modal/ConfirmationModal";

export default function Recipe() {
  // Typescript doesn't know that the id can't be undefined.
  // https://stackoverflow.com/questions/59085911/required-url-param-on-react-router-v5-with-typescript-can-be-undefined
  const { id } = useParams() as { id: string };
  const navigate = useNavigate();
  const { mutate: deleteRecipe, isLoading: isDeleting } = useDeleteRecipe();
  const [isModalOpen, setIsModalOpen] = useState(false);

  // TODO: Handle 404 error boundary or not?
  const { recipe, isLoading, isError } = useRecipe(id);

  const handleDelete = async () => {
    deleteRecipe(id, {
      onSuccess: () => navigate("/"),
    });
  };

  const FourPointGridItem = ({
    children,
    image,
  }: {
    children: ReactNode;
    image?: boolean;
  }) => {
    return (
      <Grid
        item
        xs={12}
        sm={6}
        md={6}
        lg={6}
        {...(image && {
          order: { xs: -1, sm: 0 },
        })}
      >
        <Stack
          direction="column"
          gap={2}
          padding={2}
          {...(image && {
            padding: { xs: 0, sm: 2 },
          })}
        >
          {children}
        </Stack>
      </Grid>
    );
  };

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
  const ingredients = recipe!.ingredients.map((ingredient, index) => {
    return (
      <ListItem key={index} disablePadding>
        <ListItemText
          primary={`${ingredient.amount} ${ingredient.unit} ${ingredient.name}`}
        />
      </ListItem>
    );
  });

  const directions = recipe?.directions.map((direction, index) => {
    return (
      <ListItem key={index} disableGutters sx={{ display: "list-item" }}>
        {direction.step}
      </ListItem>
    );
  });

  return (
    <>
      <PageContainer sx={{ mt: 0, p: 0 }}>
        <Grid container spacing={2}>
          <FourPointGridItem>
            <Stack direction="row" alignItems="baseline" gap={2}>
              <Typography variant="h5" gutterBottom>
                {recipe!.name}
              </Typography>
              <IconButton
                aria-label="Edit recipe"
                onClick={() => navigate(`edit`)}
              >
                <Edit />
              </IconButton>
              <IconButton
                aria-label="Delete recipe"
                onClick={() => setIsModalOpen(true)}
              >
                <Delete />
              </IconButton>
            </Stack>
            <Typography color="GrayText">{recipe!.description}</Typography>
          </FourPointGridItem>
          <FourPointGridItem image>
            <RecipeImage src={recipe?.image} />
          </FourPointGridItem>
          <FourPointGridItem>
            <Typography variant="h6">Ingredients</Typography>
            <List disablePadding>{ingredients}</List>
          </FourPointGridItem>
          <FourPointGridItem>
            <Box>
              <Typography variant="h6" gutterBottom>
                Directions
              </Typography>
              <List
                component="ol"
                sx={{
                  listStyleType: "number",
                  listStylePosition: "inside",
                }}
              >
                {directions}
              </List>
            </Box>
          </FourPointGridItem>
        </Grid>
      </PageContainer>
      <ConfirmationModal
        title="Delete recipe?"
        description="Are you sure you want to delete this recipe?"
        isOpen={isModalOpen}
        isPending={isDeleting}
        onConfirm={handleDelete}
        handleClose={() => setIsModalOpen(false)}
      />
    </>
  );
}
