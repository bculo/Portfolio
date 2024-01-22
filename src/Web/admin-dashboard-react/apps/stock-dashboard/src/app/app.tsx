import { NavLink } from 'react-router-dom';
import { AppNavigation } from './app-navigation';
import { Outlet } from 'react-router-dom';
import { useAuth } from 'react-oidc-context';
import { useEffect, useState } from 'react';

export function App() {
  const auth = useAuth();
  const [loginAttempted, setLoginAttempted] = useState(false);

  useEffect(() => {
    if (
      !loginAttempted &&
      !auth.isAuthenticated &&
      !auth.activeNavigator &&
      !auth.isLoading
    ) {
      setLoginAttempted(true);
      auth.signinSilent();
    }
  }, [auth, loginAttempted]);

  useEffect(() => {
    if (auth.user) {
      console.log(auth.user);
    }
  }, [auth.user]);

  return (
    <div>
      <ul>
        {AppNavigation.map((item) => (
          <li key={item.name}>
            <NavLink to={item.path}>{item.text}</NavLink>
          </li>
        ))}
      </ul>
      <main>
        {' '}
        <Outlet />
      </main>
    </div>
  );
}

export default App;
