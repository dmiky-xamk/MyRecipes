import { Typography, useTheme } from "@mui/material";
import PageContainer from "../../features/ui/main/PageContainer";
import { Link as RouterLink } from "react-router-dom";

export default function NotFound() {
  const theme = useTheme();

  return (
    <PageContainer center={true}>
      <Typography variant="h5">Oops, there is nothing here</Typography>
      <Typography variant="h6">
        <RouterLink
          to="/"
          style={{
            textDecoration: "none",
            color: theme.palette.primary.main,
          }}
        >
          Back to home
        </RouterLink>
      </Typography>
    </PageContainer>
  );
}
