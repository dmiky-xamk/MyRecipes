import { Button, Stack, Typography } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

export default function Index() {
  return (
    <Stack alignItems="center" justifyContent="center" flex={1} gap={2}>
      <Typography variant="h4">My Recipes</Typography>
      <Stack direction="row" gap={1}>
        <Button
          variant="contained"
          size="large"
          component={RouterLink}
          to="account/register"
        >
          Register
        </Button>
        <Button
          variant="outlined"
          size="large"
          component={RouterLink}
          to="account/login"
        >
          Sign in
        </Button>
      </Stack>
    </Stack>
  );
}
