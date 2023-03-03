import { Add } from "@mui/icons-material";
import { Fab } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function CreateRecipeFab() {
  const navigate = useNavigate();

  return (
    <Fab
      color="primary"
      aria-label="Create a new recipe"
      sx={{
        position: "fixed",
        right: (theme) => theme.spacing(1),
        bottom: (theme) => `calc(56px + ${theme.spacing(1)})`,
      }}
      onClick={() => navigate("recipe/create")}
    >
      <Add />
    </Fab>
  );
}
