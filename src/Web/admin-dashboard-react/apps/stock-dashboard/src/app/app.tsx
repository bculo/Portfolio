import React, { useEffect } from 'react';
import { hasAuthParams, useAuth } from 'react-oidc-context';
import { useDispatch } from 'react-redux';
import authSlice, { setToken } from '../stores/auth/auth-slice';
import {
  useFilterStocksQuery,
  useLazyFilterStocksQuery,
} from '../stores/api/generated';

export function App() {
  const dispatch = useDispatch();
  const auth = useAuth();
  const [trigger, { data }] = useLazyFilterStocksQuery();
  const [hasTriedSignin, setHasTriedSignin] = React.useState(false);

  console.log('RENDER');

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
    if (auth.isAuthenticated) {
      dispatch(setToken(auth.user!.access_token));
    }
  }, [auth, dispatch]);

  const fetchItems = () => {
    trigger({
      'Symbol.Value': 'A',
      'ActivityStatus.Value': 999,
      page: 1,
      take: 1,
    });
  };

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

  return (
    <div>
      <button onClick={() => fetchItems()}>FETCH</button>
      <button onClick={() => auth.signoutPopup()}>Log out</button>
    </div>
  );
}

export default App;
