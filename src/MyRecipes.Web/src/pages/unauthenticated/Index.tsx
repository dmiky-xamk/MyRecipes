import { Box, Button, Stack, Typography } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";
import { useTestUser } from "../../features/auth/auth";

export default function Index() {
  const createTestUser = useTestUser();

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
      <Box textAlign="center">
        <Button
          variant="text"
          size="medium"
          onClick={() => createTestUser.mutate()}
        >
          Try out the application
        </Button>
        <Typography variant="body2">
          (The changes you make won't be saved)
        </Typography>
      </Box>
    </Stack>
  );
}
