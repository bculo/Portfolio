import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom/client';
import { store } from './stores/store';
import { AuthProvider } from 'react-oidc-context';
import App from './app/app';
import { User } from 'oidc-client-ts';
import { oidcConfig } from './environments/oAuth2Config';

const onSigninCallback = (_: User | void) => {
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
