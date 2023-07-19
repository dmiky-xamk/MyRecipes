import {
  Card,
  CardContent,
  CardMedia,
  Theme,
  Typography,
  useMediaQuery,
} from "@mui/material";
import { Recipe } from "./recipe";

interface Props {
  recipe: Recipe;
}

export default function RecipeCard({ recipe }: Props) {
  const { name, description, image } = recipe;

  const isSmallScreen = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("sm")
  );

  const imageSrc = image
    ? require(`../../assets/${image}`)
    : require("../../assets/200.jpg");

  return (
    <Card
      sx={
        isSmallScreen
          ? {
              display: "flex",
              maxHeight: 130,
              width: "100%",
            }
          : {
              display: "flex",
              flexDirection: "column",
              flex: 1,
            }
      }
    >
      <CardMedia
        component="img"
        sx={isSmallScreen ? { maxWidth: 130, height: 130 } : { height: 200 }}
        image={imageSrc}
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
