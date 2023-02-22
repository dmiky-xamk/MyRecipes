import React, { Fragment } from "react";
import { ErrorBoundary } from "react-error-boundary";
import { Navigate, Route, Routes } from "react-router-dom";
import ErrorFallback from "./features/ui/error/ErrorFallback";
import Index from "./pages/authenticated/Index";

export default function AuthenticatedApp() {
  return (
    <Fragment>
      {/* ErrorFallback has navigation built inside, but we still need to have 'onReset' to reset the error state. */}
      <ErrorBoundary FallbackComponent={ErrorFallback} onReset={() => {}}>
        <Routes>
          <Route index path="/" element={<Index />} />
          <Route
            path="/account/*"
            element={<Navigate replace={true} to="/" />}
          />
        </Routes>
      </ErrorBoundary>
    </Fragment>
  );
}
