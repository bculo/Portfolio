import { environment } from './environment';
import { WebStorageStateStore } from "oidc-client-ts";


export const oidcConfig = {
    authority: environment.oAuth2Config.authority,
    client_id: environment.oAuth2Config.client_id,
    redirect_uri: environment.oAuth2Config.redirect_uri,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
  };