import { Fragment } from "react";
import AuthenticatedApp from "./AuthenticatedApp";
import { AuthLoader } from "./features/auth/auth";
import FullPageSpinner from "./features/ui/loading/FullPageSpinner";
import UnauthenticatedApp from "./UnauthenticatedApp";

function App() {
  // TODO: Get recipes and cache in 'Recipe' page (query)
  // TODO: Cache recipes when calling 'useUser' HOW ??
  // TODO: Bottom navigation with FAB
  // TODO: 'Search' page with fuzzy autocomplete to search for cached recipes using ingredients as search parameters
  // TODO: 'Shopping cart' page to mark which ingredients to buy (or complete recipes?)
  // TODO: A way to log out
  // TODO: Api returns recipe id as a string, remove toString() methods
  // TODO: Ingredient index for adding and removing?
  // TODO: Clear indication on the required / optional fields?

  // TODO: Api id to string, is string in database.
  // TODO: Amount to string in db.
  // TODO: Have api return the updated recipe?

  return (
    <Fragment>
      <AuthLoader
        renderLoading={() => (
          <FullPageSpinner text="Opening the application..." />
        )}
        renderUnauthenticated={() => <UnauthenticatedApp />}
      >
        <AuthenticatedApp />
      </AuthLoader>
    </Fragment>
  );
}

export default App;
