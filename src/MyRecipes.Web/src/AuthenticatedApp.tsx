import { AppBar, Button, Stack, Toolbar } from "@mui/material";
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

export default function AuthenticatedApp() {
  // TODO: Populate when querying the user
  const { isLoading } = useRecipes();
  const navigate = useNavigate();
  const logout = useLogout({ onSuccess: () => navigate("/") });

  if (isLoading) {
    return (
      <Fragment>
        <FullPageSpinner text="Loading your recipes..." />
      </Fragment>
    );
  }

  return (
    <Stack flex={1} height="100dvh">
      <AppBar position="sticky" color="default">
        <Toolbar sx={{ justifyContent: "end" }}>
          <Button color="inherit" onClick={logout.mutate}>
            Log out
          </Button>
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
      <BottomNav />
    </Stack>
  );
}
