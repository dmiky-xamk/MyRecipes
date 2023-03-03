import { Delete } from "@mui/icons-material";
import { IconButton, Stack, styled, TextField } from "@mui/material";
import { Fragment } from "react";
import { Control, Controller, FieldErrors } from "react-hook-form";
import { FormFields } from "./recipe";

const FormHelperTextStyle = {
  style: {
    backgroundColor: "#f7f7f7",
    margin: 0,
    paddingLeft: 10,
  },
};

const IngredientEditTextField = styled(TextField)({
  variant: "outlined",
  type: "text",
  backgroundColor: "#fff",
  alignSelf: "start",
  zIndex: 0, // Else shows on top of the bottom navigation
});

export default function IngredientEditFields({
  isDisabled,
  control,
  index,
  errors,
  onRemove,
}: {
  isDisabled: boolean;
  control: Control<FormFields>;
  errors: FieldErrors<FormFields>;
  index: number;
  onRemove: () => void;
}) {
  return (
    <Stack direction="row" columnGap={1}>
      <Controller
        name={`fieldsArray.${index}.amount`}
        control={control}
        render={({ field }) => (
          <IngredientEditTextField
            {...field}
            id="ingredient-amount"
            label="Amount"
            sx={{ flex: "1 1 0" }}
          />
        )}
      />
      <Controller
        name={`fieldsArray.${index}.unit`}
        control={control}
        render={({ field }) => (
          <IngredientEditTextField
            {...field}
            inputProps={{
              autoCapitalize: "none",
              style: {
                textTransform: "lowercase",
              },
            }}
            id="ingredient-unit"
            label="Unit"
            sx={{ flex: "1 1 0" }}
          />
        )}
      />
      <Controller
        name={`fieldsArray.${index}.name`}
        control={control}
        rules={{
          validate: (value: string) =>
            value.trim().length > 0 || "Name is required",
        }}
        render={({ field }) => (
          <Fragment>
            <IngredientEditTextField
              {...field}
              id="ingredient-name"
              label="Name"
              sx={{ flex: "2 1 0" }}
              error={Boolean(errors?.fieldsArray?.[index]?.name)}
              helperText={errors?.fieldsArray?.[index]?.name?.message}
              FormHelperTextProps={FormHelperTextStyle}
            />
          </Fragment>
        )}
      />

      <IconButton
        aria-label="Remove ingredient"
        onClick={onRemove}
        disabled={isDisabled}
      >
        <Delete />
      </IconButton>
    </Stack>
  );
}
