import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Stack,
  styled,
  TextField,
  Typography,
} from "@mui/material";
import { Fragment } from "react";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import IngredientEditFields from "../../features/recipes/IngredientEditFields";
import {
  FormFields,
  Recipe,
  useRecipe,
  useUpdateRecipe,
} from "../../features/recipes/recipe";
import { FormHelperTextStyle } from "../../features/recipes/RecipeEditTextField";
import RecipePlaceholderImage from "../../features/recipes/RecipePlaceholderImage";
import FullPageSpinner from "../../features/ui/loading/FullPageSpinner";
import PageContainer from "../../features/ui/main/PageContainer";

const RecipeEditTextField = styled(TextField)({
  variant: "outlined",
  backgroundColor: "#fff",
  zIndex: 0, // Else shows on top of the bottom navigation
});

export default function EditRecipe() {
  // Typescript doesn't know that the id can't be undefined.
  // https://stackoverflow.com/questions/59085911/required-url-param-on-react-router-v5-with-typescript-can-be-undefined
  const { id } = useParams() as { id: string };
  const navigate = useNavigate();

  // TODO: Handle 404 error boundary or not?
  const { recipe, isLoading, isError } = useRecipe(id);
  const {
    mutate: updateRecipe,
    isLoading: isRecipeUpdating,
    error: recipeUpdateError,
  } = useUpdateRecipe();

  // Form validation.
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm({
    mode: "onBlur",
    defaultValues: {
      recipeName: recipe?.name || "",
      recipeDescription: recipe?.description || "",
      recipeDirections: "",
      fieldsArray:
        recipe?.ingredients.map((ing) => {
          return { amount: ing.amount, unit: ing.unit, name: ing.name };
        }) || [],
    },
  });

  // Form validation for the ingredient fields.
  const { fields, append, remove } = useFieldArray({
    control,
    name: "fieldsArray",
  });

  // Each ingredient has three fields: amount, unit, name (only the name is required).
  const recipeIngredients = fields.map((ingredient, ind) => {
    return (
      <IngredientEditFields
        key={ingredient.id}
        control={control}
        index={ind}
        errors={errors}
        isDisabled={false}
        onRemove={() => removeIngredient(ind)}
      />
    );
  });

  const addIngredient = () => {
    append({ amount: "", unit: "", name: "" });
  };

  const removeIngredient = (index: number) => {
    remove(index);
  };

  const onSubmit = (values: FormFields) => {
    console.log("Submitted values", values);
    const editedRecipe: Recipe = {
      id: recipe!.id,
      name: values.recipeName,
      description: values.recipeDescription,
      ingredients: values.fieldsArray,
      image: "",
    };

    updateRecipe(editedRecipe, {
      onSuccess: () => navigate(`/recipe/${recipe!.id}`),
    });
  };

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
      <Box component="form" onSubmit={handleSubmit(onSubmit)}>
        {recipe!.image || <RecipePlaceholderImage />}
        <Stack padding={2} direction="column" gap={2}>
          <Stack gap={2}>
            <Controller
              name="recipeName"
              control={control}
              rules={{
                validate: (value: string) =>
                  value.trim().length > 0 || "Name is required",
              }}
              render={({ field }) => (
                <RecipeEditTextField
                  {...field}
                  id="recipeName"
                  label="Recipe name"
                  error={
                    Boolean(errors?.recipeName) ||
                    Boolean(recipeUpdateError?.errors?.Name)
                  }
                  helperText={
                    errors?.recipeName?.message ||
                    recipeUpdateError?.errors?.Name?.at(0)
                  }
                  FormHelperTextProps={FormHelperTextStyle}
                />
              )}
            />
            <Controller
              name="recipeDescription"
              control={control}
              rules={{
                validate: (value: string) =>
                  value.trim().length > 0 || "Name is required",
              }}
              render={({ field }) => (
                <RecipeEditTextField
                  {...field}
                  id="recipe-description"
                  label="Recipe description (optional)"
                />
              )}
            />
          </Stack>
          <Stack gap={2}>
            <Typography variant="h6">Ingredients</Typography>
            <Stack gap={1.5}>{recipeIngredients}</Stack>
            {recipeUpdateError?.errors?.Ingredients && (
              <Alert severity="error">
                {recipeUpdateError?.errors?.Ingredients?.at(0)}
              </Alert>
            )}
            <Button variant="outlined" onClick={addIngredient}>
              Add ingredient
            </Button>
          </Stack>
          <Controller
            name="recipeDirections"
            control={control}
            render={({ field }) => (
              <RecipeEditTextField
                {...field}
                id="recipe-directions"
                label="Recipe directions (optional)"
                multiline
                rows={4}
                sx={{ mt: 2 }}
              />
            )}
          />
          {recipeUpdateError && (
            <Alert severity="error">{recipeUpdateError.title}</Alert>
          )}
          <Button type="submit" variant="contained" disabled={isRecipeUpdating}>
            {isRecipeUpdating ? (
              <CircularProgress size={25} />
            ) : (
              "Tallenna resepti"
            )}
          </Button>
        </Stack>
      </Box>
    </PageContainer>
  );
}
