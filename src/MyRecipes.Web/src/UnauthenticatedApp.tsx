import { Route, Routes } from "react-router-dom";
import Login from "./pages/unauthenticated/Login";
import Register from "./pages/unauthenticated/Register";
import Index from "./pages/unauthenticated/Index";
import { Fragment } from "react";
import { ErrorBoundary } from "react-error-boundary";
import ErrorFallback from "./features/ui/error/ErrorFallback";
import NotFound from "./pages/unauthenticated/NotFound";

export default function UnauthenticatedApp() {
  return (
    <Fragment>
      {/* ErrorFallback has navigation built inside, but we still need to have 'onReset' to reset the error state. */}
      <ErrorBoundary FallbackComponent={ErrorFallback} onReset={() => {}}>
        <Routes>
          <Route index path="/" element={<Index />} />
          <Route path="account/register" element={<Register />} />
          <Route path="account/login" element={<Login />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </ErrorBoundary>
    </Fragment>
  );
}
