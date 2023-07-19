import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { createTheme } from "@mui/material/styles";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter } from "react-router-dom";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import "./base.css";
import ScrollToTop from "./scrollToTop";

// Create a baseline MUI theme for the application.
const theme = createTheme({
  palette: {
    background: {
      default: "#f7f7f7",
    },
    primary: {
      main: "#ED6E06",
    },
  },
  typography: {
    fontFamily: ["Lato", "sans-serif"].join(","),
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          "&.active": {
            color: "#ED6E06",
          },
        },
      },
    },
  },
});

// Create a 'QueryClient' to set up default options for the caching.
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      useErrorBoundary: true,
      refetchOnWindowFocus: false,
    },
  },
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

root.render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <QueryClientProvider client={queryClient}>
        <ReactQueryDevtools initialIsOpen={false} />
        <BrowserRouter>
          <ScrollToTop />
          <App />
        </BrowserRouter>
      </QueryClientProvider>
    </ThemeProvider>
  </React.StrictMode>
);
