import {
  AppBar,
  Button,
  Stack,
  Theme,
  Toolbar,
  useMediaQuery,
} from "@mui/material";
import { Fragment } from "react";
import { ErrorBoundary } from "react-error-boundary";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import { useLogout } from "./features/auth/auth";
import { useRecipes } from "./features/recipes/recipe";
import ErrorFallback from "./features/ui/error/ErrorFallback";
import FullPageSpinner from "./features/ui/loading/FullPageSpinner";
import BottomNav from "./features/ui/navigation/BottomNav";
import CreateRecipe from "./pages/authenticated/CreateRecipe";
import EditRecipe from "./pages/authenticated/EditRecipe";
import Index from "./pages/authenticated/Index";
import Recipe from "./pages/authenticated/Recipe";
import Search from "./pages/authenticated/Search";
import ShoppingCart from "./pages/authenticated/ShoppingCart";
import NotFound from "./pages/unauthenticated/NotFound";
import { NavLink } from "react-router-dom";

export default function AuthenticatedApp() {
  const { isLoading } = useRecipes();
  const navigate = useNavigate();
  const logout = useLogout({ onSuccess: () => navigate("/") });

  const isSmallScreen = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("sm")
  );

  if (isLoading) {
    return (
      <Fragment>
        <FullPageSpinner text="Loading your recipes..." />
      </Fragment>
    );
  }

  return (
    <Stack flex={1}>
      <AppBar position="sticky" color="default">
        <Toolbar sx={{ justifyContent: "end" }}>
          {isSmallScreen ? (
            <Button color="inherit" onClick={() => logout.mutate()}>
              Log out
            </Button>
          ) : (
            <>
              <Stack direction="row" flex={1} gap={2}>
                <Button color="inherit" component={NavLink} to="/">
                  Recipes
                </Button>
                <Button color="inherit" component={NavLink} to="search">
                  Search
                </Button>
                <Button color="inherit" component={NavLink} to="cart">
                  Cart
                </Button>
              </Stack>
              <Button color="inherit" onClick={() => logout.mutate()}>
                Log out
              </Button>
            </>
          )}
        </Toolbar>
      </AppBar>
      {/* ErrorFallback has navigation built inside, but we still need to have 'onReset' to reset the error state. */}
      <ErrorBoundary FallbackComponent={ErrorFallback} onReset={() => {}}>
        <Routes>
          <Route index path="/" element={<Index />} />
          <Route path="recipe/create" element={<CreateRecipe />} />
          <Route path="recipe/:id" element={<Recipe />} />
          <Route path="recipe/:id/edit" element={<EditRecipe />} />
          <Route path="search" element={<Search />} />
          <Route path="cart" element={<ShoppingCart />} />
          <Route
            path="account/*"
            element={<Navigate replace={true} to="/" />}
          />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </ErrorBoundary>
      {isSmallScreen && <BottomNav />}
    </Stack>
  );
}
