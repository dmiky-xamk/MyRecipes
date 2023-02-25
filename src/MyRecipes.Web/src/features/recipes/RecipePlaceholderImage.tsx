import { Box } from "@mui/material";

export default function RecipePlaceholderImage() {
  return (
    <Box
      component="img"
      sx={{ maxHeight: 200, objectFit: "cover" }}
      width="100%"
      alt="Recipe placeholder image"
      src="https://via.placeholder.com/200"
    />
  );
}
