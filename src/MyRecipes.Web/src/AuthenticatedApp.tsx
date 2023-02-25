import { Stack } from "@mui/material";
import React, { Fragment } from "react";
import { ErrorBoundary } from "react-error-boundary";
import { Navigate, Route, Routes } from "react-router-dom";
import { useRecipes } from "./features/recipes/recipe";
import ErrorFallback from "./features/ui/error/ErrorFallback";
import FullPageSpinner from "./features/ui/loading/FullPageSpinner";
import BottomNav from "./features/ui/navigation/BottomNav";
import EditRecipe from "./pages/authenticated/EditRecipe";
import Index from "./pages/authenticated/Index";
import Recipe from "./pages/authenticated/Recipe";
import Search from "./pages/authenticated/Search";
import NotFound from "./pages/unauthenticated/NotFound";

export default function AuthenticatedApp() {
  // TODO: Populate when querying the user
  const { isLoading } = useRecipes();

  if (isLoading) {
    return (
      <Fragment>
        <FullPageSpinner text="Loading your recipes..." />
      </Fragment>
    );
  }

  return (
    <Stack justifyContent="space-between" flex={1}>
      {/* ErrorFallback has navigation built inside, but we still need to have 'onReset' to reset the error state. */}
      <ErrorBoundary FallbackComponent={ErrorFallback} onReset={() => {}}>
        <Routes>
          <Route index path="/" element={<Index />} />
          <Route path="recipe/:id" element={<Recipe />} />
          <Route path="recipe/:id/edit" element={<EditRecipe />} />
          <Route path="search" element={<Search />} />
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
