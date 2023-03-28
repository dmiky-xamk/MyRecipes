import { Fragment } from "react";
import AuthenticatedApp from "./AuthenticatedApp";
import { useUser } from "./features/auth/auth";
import FullPageSpinner from "./features/ui/loading/FullPageSpinner";
import UnauthenticatedApp from "./UnauthenticatedApp";

function App() {
  // TODO: 'Shopping cart' page to mark which ingredients to buy (or complete recipes?)

  // TODO: Ingredient index for adding and removing?
  // TODO: Clear indication on the required / optional fields?

  // TODO: Navigation to home when updating / creating recipe?

  const { user, isLoading } = useUser();

  console.log("App user:", user);
  console.log("IsLoading:", isLoading);

  if (isLoading) {
    return <FullPageSpinner text="Opening the application..." />;
  }

  return (
    <Fragment>{user ? <AuthenticatedApp /> : <UnauthenticatedApp />}</Fragment>
  );
}

export default App;
