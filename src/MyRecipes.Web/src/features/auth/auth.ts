import { configureAuth } from "react-query-auth";
import agent, { ApiErrorResponse, updateAxiosToken } from "../../api/agent";

export interface AuthCredentials {
  email: string;
  password: string;
}

export interface User {
  token: string;
  recipes: [];
}

const userFn = async (): Promise<User | null> => {
  const token = localStorage.getItem("token");

  if (token) {
    updateAxiosToken(token);
    return agent.Account.user().then(
      // Token is valid, the user is authenticated.
      (recipes) => {
        return { token: token, recipes: recipes };
      },
      // Remove the token and return null if the token validation fails (API sends 401).
      (_) => {
        localStorage.removeItem("token");
        return null;
      }
    );
  }

  // Token doesn't exist, the user isn't authenticated.
  return null;
};

const handleAuthResponse = (token: string): User => {
  updateAxiosToken(token);
  localStorage.setItem("token", token);

  // TODO: Get the recipes when logging in?
  return { token: token, recipes: [] };
};

const loginFn = async (credentials: AuthCredentials) => {
  const token: string = await agent.Account.login(credentials);
  return handleAuthResponse(token);
};

const registerFn = async (credentials: AuthCredentials) => {
  const token: string = await agent.Account.register(credentials);
  return handleAuthResponse(token);
};

const logoutFn = () => {
  updateAxiosToken(null);
  localStorage.removeItem("token");

  return Promise.resolve();
};

const { useUser, useLogin, useRegister, useLogout, AuthLoader } = configureAuth<
  User | null,
  ApiErrorResponse,
  AuthCredentials,
  AuthCredentials
>({
  userFn: () => userFn(),
  loginFn: (credentials: AuthCredentials) => loginFn(credentials),
  registerFn: (credentials: AuthCredentials) => registerFn(credentials),
  logoutFn: () => logoutFn(),
});

export { useUser, useLogin, useRegister, useLogout, AuthLoader };
