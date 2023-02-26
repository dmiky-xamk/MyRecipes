import {
  Card,
  CardContent,
  Divider,
  ListItem,
  Stack,
  Typography,
} from "@mui/material";
import { MatchedRecipe } from "./recipe";

export default function RecipeSearchCard({
  recipe,
  matchingIngredients,
  nonMatchingIngredients,
  totalIngredientsCount,
}: MatchedRecipe) {
  return (
    <ListItem>
      <Card sx={{ flex: 1 }}>
        <CardContent>
          <Stack direction="row" justifyContent="space-between">
            <Typography variant="h6" gutterBottom>
              {recipe.name}
            </Typography>
            <Typography>{`${matchingIngredients.length} / ${totalIngredientsCount}`}</Typography>
          </Stack>
          <Stack direction="row" gap={2} flexWrap="wrap">
            {matchingIngredients.map((ing) => {
              return <Typography key={ing.name}>{ing.name}</Typography>;
            })}
          </Stack>
          <Divider sx={{ my: 1 }} />
          <Stack direction="row" gap={2} flexWrap="wrap">
            {nonMatchingIngredients.map((ing) => {
              return (
                <Typography key={ing.name} variant="body2" color="GrayText">
                  {ing.name}
                </Typography>
              );
            })}
          </Stack>
        </CardContent>
      </Card>
    </ListItem>
  );
}
