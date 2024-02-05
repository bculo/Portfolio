import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom/client';
import { store } from './stores/store';
import { AuthProvider } from 'react-oidc-context';
import App from './app/app';
import { User, WebStorageStateStore } from 'oidc-client-ts';
import { oAuth2Config } from './configs/oauth-config';

const oidcConfig = {
  authority: oAuth2Config.authority,
  client_id: oAuth2Config.client_id,
  redirect_uri: oAuth2Config.redirect_uri,
  userStore: new WebStorageStateStore({ store: window.localStorage }),
};

const onSigninCallback = (user: User | void) => {
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
