import { CircularProgress, Stack, Typography } from "@mui/material";
import PageContainer from "../main/PageContainer";

export default function FullPageSpinner() {
  return (
    <PageContainer center={true}>
      <Stack alignItems="center" justifyContent="center" gap={4}>
        <Typography variant="h5">Opening the application...</Typography>
        <CircularProgress />
      </Stack>
    </PageContainer>
  );
}
