import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom/client';
import { store } from './stores/store';
import { AuthProvider } from 'react-oidc-context';
import App from './app/app';
import { User, WebStorageStateStore } from 'oidc-client-ts';

const oidcConfig = {
  authority: 'http://localhost:8080/realms/PortfolioRealm/',
  client_id: 'Stock.Client',
  redirect_uri: 'http://localhost:4200/callback',
  userStore: new WebStorageStateStore({ store: window.localStorage }),
};

const onSigninCallback = (user: User | void) => {
  console.log(user);
  window.history.replaceState({}, document.title, window.location.pathname);
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StrictMode>
    <AuthProvider {...oidcConfig} onSigninCallback={onSigninCallback}>
      <Provider store={store}>
        <App />
      </Provider>
    </AuthProvider>
  </StrictMode>
);
