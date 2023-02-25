import { Box, Card, CardContent, CardMedia, Typography } from "@mui/material";
import { Recipe } from "./recipe";

interface Props {
  recipe: Recipe;
}

export default function RecipeCard({ recipe }: Props) {
  const { id, name, description, image, ingredients } = recipe;

  return (
    <Card sx={{ display: "flex", maxHeight: 130, width: "100%" }}>
      <CardMedia
        component="img"
        sx={{ width: 130 }}
        image="https://via.placeholder.com/130" // {image}
        alt="Recipe image"
      />
      <CardContent>
        <Typography
          variant="h6"
          sx={{ lineHeight: 1.35, hyphens: "auto" }}
          gutterBottom
        >
          {name}
        </Typography>
        <Typography
          variant="subtitle1"
          color="GrayText"
          component="div"
          lineHeight={1.5}
          sx={{
            whiteSpace: "pre-wrap",
            overflow: "hidden",
            textOverflow: "ellipsis",
            display: "-webkit-box",
            WebkitLineClamp: "2",
            WebkitBoxOrient: "vertical",
          }}
        >
          {description}
        </Typography>
      </CardContent>
    </Card>
  );
}
