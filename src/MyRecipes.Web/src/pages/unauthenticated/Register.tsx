import { Typography } from "@mui/material";
import { useRegister } from "../../features/auth/auth";
import AuthForm from "../../features/auth/AuthForm";
import PageContainer from "../../features/ui/main/PageContainer";

export default function Register() {
  const register = useRegister();

  return (
    <PageContainer maxWidth="xs" center={true}>
      <Typography variant="h5" mb={4}>
        Create a new account
      </Typography>
      <AuthForm authMode="register" handleAuth={register} />
    </PageContainer>
  );
}
