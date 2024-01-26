import React, { useEffect } from 'react';
import { hasAuthParams, useAuth } from 'react-oidc-context';
import { useDispatch } from 'react-redux';
import authSlice, { setToken } from '../stores/auth/auth-slice';

export function App() {
  const dispatch = useDispatch();
  const auth = useAuth();
  const [hasTriedSignin, setHasTriedSignin] = React.useState(false);

  // automatically sign-in
  React.useEffect(() => {
    if (
      !hasAuthParams() &&
      !auth.isAuthenticated &&
      !auth.activeNavigator &&
      !auth.isLoading &&
      !hasTriedSignin
    ) {
      auth.signinSilent();
      setHasTriedSignin(true);
    }
  }, [auth, hasTriedSignin]);

  useEffect(() => {
    console.log(auth);
    if (auth.isAuthenticated) {
      dispatch(setToken(auth.user!.access_token));
    }
  }, [auth, dispatch]);

  if (auth.isLoading) {
    return <div>LOADING.......</div>;
  }

  if (!auth.isAuthenticated) {
    return (
      <div>
        {' '}
        <button onClick={() => auth.signinRedirect()}>Log in</button>
      </div>
    );
  }

  return <button onClick={() => auth.signoutPopup()}>Log out</button>;
}

export default App;
