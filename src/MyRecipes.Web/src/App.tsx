import { Fragment } from "react";
import AuthenticatedApp from "./AuthenticatedApp";
import { AuthLoader } from "./features/auth/auth";
import FullPageSpinner from "./features/ui/loading/FullPageSpinner";
import UnauthenticatedApp from "./UnauthenticatedApp";

function App() {
  return (
    <Fragment>
      <AuthLoader
        renderLoading={() => <FullPageSpinner />}
        renderUnauthenticated={() => <UnauthenticatedApp />}
      >
        <AuthenticatedApp />
      </AuthLoader>
    </Fragment>
  );
}

export default App;
