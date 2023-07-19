import { Delete } from "@mui/icons-material";
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  IconButton,
  Stack,
  styled,
  TextField,
  Typography,
} from "@mui/material";
import { UseMutationResult } from "@tanstack/react-query";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import IngredientEditFields from "../../features/recipes/IngredientEditFields";
import {
  FormFields,
  Recipe,
  RecipeErrorResponse,
} from "../../features/recipes/recipe";
import { FormHelperTextStyle } from "../../features/recipes/RecipeEditTextField";
import RecipeImage from "./RecipeImage";

const RecipeEditTextField = styled(TextField)({
  variant: "outlined",
  backgroundColor: "#fff",
  zIndex: 0, // Else shows on top of the bottom navigation
});

interface Props {
  mutate: UseMutationResult<Recipe, RecipeErrorResponse, Recipe>;
  recipe?: Recipe;
}

export default function RecipeForm({ mutate, recipe }: Props) {
  const navigate = useNavigate();

  const { mutate: mutateRecipe, isLoading, error: recipeError } = mutate;

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
      fieldsArray: recipe?.ingredients.map((ing) => {
        return { amount: ing.amount, unit: ing.unit, name: ing.name };
      }) || [{ amount: "", unit: "", name: "" }],
      directionsArray: recipe?.directions.map((dir) => {
        return { step: dir.step };
      }) || [{ step: "" }],
    },
  });

  // Form validation for the ingredient fields.
  const {
    fields,
    append: appendIngredient,
    remove: removeIngredient,
  } = useFieldArray({
    control,
    name: "fieldsArray",
  });

  const {
    fields: directionFields,
    append: appendStep,
    remove: removeStep,
  } = useFieldArray({
    control,
    name: "directionsArray",
  });

  // Each ingredient has three fields: amount, unit, name (only the name is required).
  const recipeIngredients = fields.map((ingredient, index) => {
    return (
      <IngredientEditFields
        key={ingredient.id}
        control={control}
        index={index}
        errors={errors}
        isDisabled={fields.length <= 1}
        onRemove={() => removeIngredient(index)}
      />
    );
  });

  const recipeDirections = directionFields.map((step, index) => {
    return (
      <Stack key={step.id} direction="row" columnGap={1}>
        <Controller
          name={`directionsArray.${index}.step`}
          control={control}
          render={({ field }) => (
            <RecipeEditTextField
              {...field}
              id="recipe-directions"
              label={`Step ${index + 1}`}
              sx={{ flex: "1" }}
            />
          )}
        />
        <IconButton aria-label="Remove step" onClick={() => removeStep(index)}>
          <Delete />
        </IconButton>
      </Stack>
    );
  });

  const addStep = () => {
    appendStep({ step: "" });
  };

  const addIngredient = () => {
    appendIngredient({ amount: "", unit: "", name: "" });
  };

  const onSubmit = (values: FormFields) => {
    const editedRecipe: Recipe = {
      id: recipe?.id || "",
      name: values.recipeName,
      description: values.recipeDescription,
      ingredients: values.fieldsArray,
      directions: values.directionsArray,
      image: "",
    };

    mutateRecipe(editedRecipe, {
      onSuccess: () => navigate("/"),
    });
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)}>
      <RecipeImage src={recipe?.image} />
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
                label="Recipe name *"
                error={
                  Boolean(errors?.recipeName) ||
                  Boolean(recipeError?.errors?.Name)
                }
                helperText={
                  errors?.recipeName?.message ||
                  recipeError?.errors?.Name?.at(0)
                }
                FormHelperTextProps={FormHelperTextStyle}
              />
            )}
          />
          <Controller
            name="recipeDescription"
            control={control}
            render={({ field }) => (
              <RecipeEditTextField
                {...field}
                id="recipe-description"
                label="Recipe description"
              />
            )}
          />
        </Stack>
        <Stack gap={2}>
          <Typography variant="h6">Ingredients</Typography>
          <Stack gap={1.5}>{recipeIngredients}</Stack>
          {recipeError?.errors?.Ingredients && (
            <Alert severity="error">
              {recipeError?.errors?.Ingredients?.at(0)}
            </Alert>
          )}
          <Button variant="outlined" onClick={addIngredient}>
            Add ingredient
          </Button>
        </Stack>
        <Typography variant="h6">Directions</Typography>
        {recipeDirections}
        <Button variant="outlined" onClick={addStep}>
          Add step
        </Button>
        {recipeError && <Alert severity="error">{recipeError.title}</Alert>}
        <Button type="submit" variant="contained" disabled={isLoading}>
          {isLoading ? <CircularProgress size={25} /> : "Save recipe"}
        </Button>
      </Stack>
    </Box>
  );
}
