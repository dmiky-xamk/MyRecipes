import { Add } from "@mui/icons-material";
import { Fab, Theme, useMediaQuery } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function CreateRecipeFab() {
  const navigate = useNavigate();

  const isSmallScreen = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("sm")
  );

  return (
    <Fab
      color="primary"
      aria-label="Create a new recipe"
      sx={{
        position: "fixed",
        right: (theme) => (isSmallScreen ? theme.spacing(1) : theme.spacing(2)),
        bottom: (theme) =>
          isSmallScreen ? `calc(56px + ${theme.spacing(1)})` : theme.spacing(2),
      }}
      onClick={() => navigate("recipe/create")}
    >
      <Add />
    </Fab>
  );
}
