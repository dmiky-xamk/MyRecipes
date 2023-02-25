import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import {
  render,
  screen,
  fireEvent,
  waitFor,
  renderHook,
} from "@testing-library/react";
import { configureAuth } from "react-query-auth";
import AuthForm from "./AuthForm";
import { BrowserRouter } from "react-router-dom";
import "@testing-library/jest-dom/extend-expect";
import { ApiErrorResponse } from "../../api/agent";
import { AuthCredentials, User } from "./auth";

// TODO: Mock API calls?

// Thanks to Alan for the great examples.
// https://github.com/alan2207/react-query-auth/blob/master/test/index.test.tsx

const renderAppHook = <Result,>(hook: () => Result) => {
  const client = new QueryClient();
  return renderHook(hook, {
    wrapper: ({ children }) => (
      <QueryClientProvider client={client}>{children}</QueryClientProvider>
    ),
  });
};

const user: User = {
  token: "test",
};

const config = {
  userFn: jest.fn(),
  loginFn: jest.fn(),
  registerFn: jest.fn(),
  logoutFn: jest.fn(),
};

const { useLogin, useRegister } = configureAuth<
  User | null,
  ApiErrorResponse,
  AuthCredentials,
  AuthCredentials
>(config);

beforeEach(() => {
  jest.resetAllMocks();
});

describe("AuthForm", () => {
  it("should display error when password is invalid", async () => {
    const { result } = renderAppHook(() => useLogin());
    const authCredentials: AuthCredentials = {
      email: "test@mail.com",
      password: "test",
    };

    const { container } = render(
      <AuthForm authMode="login" handleAuth={result.current} />,
      {
        wrapper: BrowserRouter,
      }
    );

    expect(await screen.findByText("Sign in")).toBeInTheDocument();

    fireEvent.input(screen.getByRole("textbox", { name: /email/i }), {
      target: {
        value: authCredentials.email,
      },
    });

    fireEvent.input(screen.getByTestId("passwordInput"), {
      target: {
        value: authCredentials.password,
      },
    });

    fireEvent.submit(
      screen.getByRole("button", {
        name: /sign in/i,
      })
    );

    await waitFor(() =>
      expect(container).toHaveTextContent(
        "The password must be between 6 to 18 characters long, and it must contain atleast one capital letter and a number."
      )
    );
  });

  it("should display error when email is invalid", async () => {
    const { result } = renderAppHook(() => useLogin());
    const authCredentials: AuthCredentials = {
      email: "testmail.com",
      password: "Test123",
    };

    const { container } = render(
      <AuthForm authMode="login" handleAuth={result.current} />,
      {
        wrapper: BrowserRouter,
      }
    );

    expect(await screen.findByText("Sign in")).toBeInTheDocument();

    fireEvent.input(screen.getByRole("textbox", { name: /email/i }), {
      target: {
        value: authCredentials.email,
      },
    });

    fireEvent.input(screen.getByTestId("passwordInput"), {
      target: {
        value: authCredentials.password,
      },
    });

    fireEvent.submit(
      screen.getByRole("button", {
        name: /sign in/i,
      })
    );

    await waitFor(() =>
      expect(container).toHaveTextContent(
        "Entered value does not match email format."
      )
    );
  });

  it("should display error when values are empty", async () => {
    const { result } = renderAppHook(() => useLogin());
    const authCredentials: AuthCredentials = {
      email: "",
      password: "",
    };

    const { container } = render(
      <AuthForm authMode="login" handleAuth={result.current} />,
      {
        wrapper: BrowserRouter,
      }
    );

    expect(await screen.findByText("Sign in")).toBeInTheDocument();

    fireEvent.input(screen.getByRole("textbox", { name: /email/i }), {
      target: {
        value: authCredentials.email,
      },
    });

    fireEvent.input(screen.getByTestId("passwordInput"), {
      target: {
        value: authCredentials.password,
      },
    });

    fireEvent.submit(
      screen.getByRole("button", {
        name: /sign in/i,
      })
    );

    await waitFor(() =>
      expect(container).toHaveTextContent("Email is required")
    );
    expect(container).toHaveTextContent("Password is required");
  });

  it("should call the login function when values are valid and set the authenticated user on success", async () => {
    const { result } = renderAppHook(() => useLogin());
    const authCredentials: AuthCredentials = {
      email: "test@mail.com",
      password: "Test123",
    };

    config.loginFn.mockResolvedValue(user);

    render(<AuthForm authMode="login" handleAuth={result.current} />, {
      wrapper: BrowserRouter,
    });

    expect(await screen.findByText("Sign in")).toBeInTheDocument();

    fireEvent.input(screen.getByRole("textbox", { name: /email/i }), {
      target: {
        value: authCredentials.email,
      },
    });

    fireEvent.input(screen.getByTestId("passwordInput"), {
      target: {
        value: authCredentials.password,
      },
    });

    fireEvent.submit(
      screen.getByRole("button", {
        name: /sign in/i,
      })
    );

    await waitFor(() =>
      expect(config.loginFn).toHaveBeenCalledWith(authCredentials)
    );

    expect(result.current.data).toEqual(user);
  });

  it("should call the register function when values are valid and set the authenticated user on success", async () => {
    const { result } = renderAppHook(() => useRegister());
    const authCredentials: AuthCredentials = {
      email: "test@mail.com",
      password: "Test123",
    };

    config.registerFn.mockResolvedValue(user);

    render(<AuthForm authMode="register" handleAuth={result.current} />, {
      wrapper: BrowserRouter,
    });

    expect(await screen.findByText("Register")).toBeInTheDocument();

    fireEvent.input(screen.getByRole("textbox", { name: /email/i }), {
      target: {
        value: authCredentials.email,
      },
    });

    fireEvent.input(screen.getByTestId("passwordInput"), {
      target: {
        value: authCredentials.password,
      },
    });

    fireEvent.submit(
      screen.getByRole("button", {
        name: /register/i,
      })
    );

    await waitFor(() =>
      expect(config.registerFn).toHaveBeenCalledWith(authCredentials)
    );

    expect(result.current.data).toEqual(user);
  });
});
