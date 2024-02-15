import { WebStorageStateStore } from "oidc-client-ts";

export const oAuth2Config = {
    authority: 'http://localhost:8080/realms/PortfolioRealm/',
    client_id: 'Stock.Client',
    redirect_uri: 'http://localhost:4200/',
};


export const oidcConfig = {
    authority: oAuth2Config.authority,
    client_id: oAuth2Config.client_id,
    redirect_uri: oAuth2Config.redirect_uri,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
  };