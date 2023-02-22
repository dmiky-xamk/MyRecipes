import { Typography } from "@mui/material";
import { useLogin } from "../../features/auth/auth";
import AuthForm from "../../features/auth/AuthForm";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Login() {
  const mutate = useLogin({
    retry: 0,
  });

  return (
    <PageContainer maxWidth="xs" center={true}>
      <Typography variant="h5" mb={4}>
        Sign in to an existing account
      </Typography>
      <AuthForm authMode="login" handleAuth={mutate} />
    </PageContainer>
  );
}
