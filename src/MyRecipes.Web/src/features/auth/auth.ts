import {
  QueryClient,
  useMutation,
  UseMutationOptions,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import agent, { ApiErrorResponse, updateAxiosToken } from "../../api/agent";
import { Recipe } from "../recipes/recipe";

export interface AuthCredentials {
  email: string;
  password: string;
}

export interface User {
  email: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  recipes: Recipe[];
}

export interface ValidationErrorResponse extends ApiErrorResponse {
  errors?: {
    Email?: [];
    Password?: [];
  };
}

const handleResponse = (res: AuthResponse, queryClient: QueryClient) => {
  // Update the token
  updateAxiosToken(res.token);
  localStorage.setItem("token", res.token);

  // Update the cache
  queryClient.setQueryData(["authenticated-user"], res.email);
  queryClient.setQueryData(["recipes"], res.recipes);
};

const useLogin = () => {
  const queryClient = useQueryClient();

  return useMutation<User | null, ValidationErrorResponse, AuthCredentials>(
    async (credentials: AuthCredentials) => {
      const res = await agent.Account.login(credentials);
      handleResponse(res, queryClient);

      // Return the user
      return { email: res.email };
    },
    {
      retry: 0,

      // Return the error so that the UI can show the information.
      onError: (err: ValidationErrorResponse) => {
        console.log("Error:", err);
        return err;
      },
    }
  );
};

const useRegister = () => {
  const queryClient = useQueryClient();

  return useMutation<User | null, ValidationErrorResponse, AuthCredentials>(
    async (credentials: AuthCredentials) => {
      const res = await agent.Account.register(credentials);
      handleResponse(res, queryClient);

      // Return the user
      return { email: res.email };
    },
    {
      retry: 0,

      // Return the error so that the UI can show the information.
      onError: (err: ValidationErrorResponse) => err,
    }
  );
};

const useLogout = (
  options?: UseMutationOptions<User | null, ValidationErrorResponse>
) => {
  const queryClient = useQueryClient();

  return useMutation<User | null, ValidationErrorResponse>(
    async () => {
      updateAxiosToken(null);
      localStorage.removeItem("token");

      queryClient.setQueryData(["authenticated-user"], null);
      queryClient.setQueryData(["recipes"], null);

      return null;
    },
    {
      onSuccess: (user, ...rest) => {
        options?.onSuccess?.(user, ...rest);
      },
    }
  );
};

const useUser = () => {
  const token = localStorage.getItem("token");
  const queryClient = useQueryClient();

  const {
    data: user,
    isInitialLoading: isLoading, // Breaking change in react-query 4.0: isLoading is true when the query is not enabled.
    isError,
    error,
  } = useQuery<User | null, ValidationErrorResponse>(
    ["authenticated-user"],
    async () => {
      updateAxiosToken(token);
      const res = await agent.Account.user();

      queryClient.setQueryData(["recipes"], res.recipes);

      return { email: res.email };
    },
    {
      enabled: !!token,
      useErrorBoundary: false,
      retry: false,
      onError: (_) => {
        localStorage.removeItem("token");
        return null;
      },
    }
  );

  return { user, isLoading, isError, error };
};

export { useUser, useLogin, useRegister, useLogout };
