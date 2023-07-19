import { Box } from "@mui/material";

interface Props {
  src?: string;
  styles?: any;
}

export default function RecipeImage({ src, styles }: Props) {
  const imageSrc = src
    ? require(`../../assets/${src}`)
    : require("../../assets/200.jpg");

  return (
    <Box maxHeight={300}>
      <Box
        component="img"
        height={300}
        sx={{ objectFit: "cover", ...styles }}
        width="100%"
        alt={src || "Recipe placeholder image"}
        src={imageSrc}
      />
    </Box>
  );
}
