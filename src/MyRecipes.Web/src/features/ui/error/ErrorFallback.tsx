import { Typography, useTheme } from "@mui/material";
import PageContainer from "../main/PageContainer";
import { Link as RouterLink } from "react-router-dom";

export default function ErrorFallback({ resetErrorBoundary }: any) {
  const theme = useTheme();

  return (
    <PageContainer center={true}>
      <Typography variant="h5">An unexpected error happened.</Typography>
      <Typography variant="h6">
        <RouterLink
          onClick={resetErrorBoundary}
          to="/"
          style={{ textDecoration: "none", color: theme.palette.primary.main }}
        >
          Back to home
        </RouterLink>
      </Typography>
    </PageContainer>
  );
}
