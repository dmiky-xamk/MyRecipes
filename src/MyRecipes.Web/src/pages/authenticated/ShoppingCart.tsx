import { Check, Delete } from "@mui/icons-material";
import { Button, IconButton, List, Stack } from "@mui/material";
import { FormEvent, Fragment, useEffect, useState } from "react";
import CartListItem from "../../features/cart/CartListItem";
import { RecipeEditTextField } from "../../features/recipes/RecipeEditTextField";
import PageContainer from "../../features/ui/main/PageContainer";

const ingredientsArr = [
  "Maitoa",
  "Voita",
  "Jauhoja",
  "Sokeria",
  "Suolaa",
  "Hapankorppuja",
  "Kurkkua",
  "Tomaattia",
  "Avokadoja",
  "Salaattia",
];

export default function ShoppingCart() {
  const [cartIngredients, setCartIngredients] = useState<string[]>(() => {
    // TODO: Temporary
    const items = localStorage.getItem("cartItems");

    return items ? JSON.parse(items) : [];
  });
  const [ingredientValue, setIngredientValue] = useState<string>("");
  const [checked, setChecked] = useState<string[]>([]);

  // TODO: Key?
  const ingredients = cartIngredients.map((ing, index) => {
    return (
      <CartListItem
        key={index}
        ingredient={ing}
        checked={checked}
        onCheck={setChecked}
      />
    );
  });

  const handleAddCartIngredient = (e: FormEvent) => {
    e.preventDefault();

    if (!ingredientValue.trim().length) {
      return;
    }

    setCartIngredients((oldIngredients) => {
      return [...oldIngredients, ingredientValue];
    });

    setIngredientValue("");
  };

  const handleDeleteChecked = () => {
    setCartIngredients((oldIngredients) => {
      return oldIngredients.filter((ing) => !checked.includes(ing));
    });
  };

  useEffect(() => {
    localStorage.setItem("cartItems", JSON.stringify(cartIngredients));
  }, [cartIngredients]);

  return (
    <Fragment>
      <PageContainer fullHeight>
        <Button
          variant="outlined"
          color="error"
          startIcon={<Delete />}
          onClick={handleDeleteChecked}
        >
          Delete selected
        </Button>
        <List>{ingredients}</List>
      </PageContainer>
      {/* {!isAddingNewIngredient ? (
        <Fab
          color="primary"
          aria-label="Add an ingredient to the cart"
          sx={{
            position: "fixed",
            right: (theme) => theme.spacing(1),
            bottom: (theme) => `calc(56px + ${theme.spacing(1)})`,
          }}
          onClick={() => setIsAddingNewIngredient(true)}
        >
          <Add />
        </Fab> */}
      {/* ) : ( */}
      <Stack
        direction="row"
        component="form"
        onSubmit={handleAddCartIngredient}
        gap={1}
        padding={1}
        sx={{
          position: "sticky",
          // bottom: (theme) => `calc(56px + ${theme.spacing(1)})`,
          bottom: "56px",
          backgroundColor: "#f7f7f7",
        }}
      >
        <RecipeEditTextField
          label="New ingredient"
          sx={{ flex: 1 }}
          value={ingredientValue}
          onChange={(e) => setIngredientValue(e.target.value)}
        />
        <IconButton
          type="submit"
          aria-label="Add ingredient"
          sx={{
            border: "1px solid green",
            borderRadius: 1,
            color: "green",
            backgroundColor: "#fff",
          }}
        >
          <Check />
        </IconButton>
      </Stack>
      {/* )} */}
    </Fragment>
  );
}
