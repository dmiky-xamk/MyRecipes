import { Visibility, VisibilityOff } from "@mui/icons-material";
import {
  Alert,
  Button,
  CircularProgress,
  FormControl,
  FormHelperText,
  IconButton,
  InputAdornment,
  InputLabel,
  OutlinedInput,
  Stack,
  TextField,
  Typography,
  useTheme,
} from "@mui/material";
import { Fragment, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Link as RouterLink, useNavigate } from "react-router-dom";
import { ApiErrorResponse } from "../../api/agent";
import { UseMutationResult } from "@tanstack/react-query";
import { User, ValidationErrorResponse } from "./auth";

const RegisterModeText = {
  submitButtonText: "Register",
  routerLinkPreText: "Already have an account?",
  routerLinkPath: "/account/login",
  routerLinkText: "Sign in",
};

const LoginModeText = {
  submitButtonText: "Sign in",
  routerLinkPreText: "Don't have an account yet?",
  routerLinkPath: "/account/register",
  routerLinkText: "Register",
};

interface AuthCredentials {
  email: string;
  password: string;
}

interface Props {
  authMode: "register" | "login";
  handleAuth: UseMutationResult<
    User | null,
    ValidationErrorResponse,
    AuthCredentials,
    unknown
  >;
}

export default function AuthForm({ authMode, handleAuth }: Props) {
  // The user can toggle to view their password
  const [showPassword, setShowPassword] = useState(false);
  const handleClickShowPassword = () => setShowPassword((show) => !show);
  const handleMouseDownPassword = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    event.preventDefault();
  };

  // Display text appropriate to the page (login / register)
  const authModeText =
    authMode === "register" ? RegisterModeText : LoginModeText;

  // Use the primary color from the theme
  const theme = useTheme();

  // Get the appropriate auth method (register, login) from the parent
  const { mutate: authenticate, isLoading, error: apiError } = handleAuth;

  // Form validation
  const {
    control,
    formState: { errors },
    handleSubmit,
  } = useForm<AuthCredentials>({
    mode: "onBlur",
    defaultValues: {
      email: "",
      password: "",
    },
  });

  // Navigate the user after succesful authentication.
  const navigate = useNavigate();

  const onSubmit = (loginValues: AuthCredentials) => {
    authenticate(loginValues, {
      onSuccess: () => navigate("/"),
    });
  };

  return (
    <Stack
      component="form"
      gap={1.2}
      alignSelf="stretch"
      onSubmit={handleSubmit(onSubmit)}
    >
      <Controller
        name="email"
        control={control}
        rules={{
          required: "Email is required",
          pattern: {
            value: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/,
            message: "Entered value does not match email format.",
          },
        }}
        render={({ field }) => (
          <TextField
            {...field}
            id="email"
            label="Email"
            type="email"
            error={Boolean(errors?.email) || Boolean(apiError?.errors?.Email)}
            helperText={
              errors?.email?.message || apiError?.errors?.Email?.at(0)
            }
          />
        )}
      />
      <Controller
        name="password"
        control={control}
        rules={{
          required: "Password is required",
          pattern: {
            value: /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,18}$/,
            message:
              "The password must be between 6 to 18 characters long, and it must contain atleast one capital letter and a number.",
          },
        }}
        render={({ field }) => (
          <Fragment>
            <FormControl
              error={
                Boolean(errors?.password) || Boolean(apiError?.errors?.Password)
              }
            >
              <InputLabel htmlFor="password">Password</InputLabel>
              <OutlinedInput
                {...field}
                id="password"
                inputProps={{
                  "data-testid": "passwordInput",
                }}
                type={showPassword ? "text" : "password"}
                endAdornment={
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPassword}
                      onMouseDown={handleMouseDownPassword}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                }
                label="Password"
              />
              <FormHelperText>
                {errors?.password?.message || apiError?.errors?.Password?.at(0)}
              </FormHelperText>
            </FormControl>
          </Fragment>
        )}
      />
      {apiError && <Alert severity="error">{apiError.title}</Alert>}
      <Button
        type="submit"
        variant="contained"
        fullWidth
        sx={{ mt: 1 }}
        disabled={isLoading}
      >
        {isLoading ? (
          <CircularProgress size={30} />
        ) : (
          authModeText.submitButtonText
        )}
      </Button>
      <Typography sx={{ mt: 2 }}>
        {authModeText.routerLinkPreText}{" "}
        <RouterLink
          to={authModeText.routerLinkPath}
          style={{
            textDecoration: "none",
            color: theme.palette.primary.main,
          }}
        >
          {authModeText.routerLinkText}
        </RouterLink>
      </Typography>
    </Stack>
  );
}
