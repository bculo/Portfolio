export const environment = {
  stockApiBase: import.meta.env.VITE_STOCK_API_URL,
  cryptoApiBase: import.meta.env.VITE_CRYPTO_API_URL,
  websocket: import.meta.env.VITE_WEBSOCKET_URL,
  oAuth2Config: {
    authority: import.meta.env.VITE_AUTH_AUTHORITY,
    client_id: import.meta.env.VITE_AUTH_CLIEND_ID,
    redirect_uri: import.meta.env.VITE_AUTH_REDIRECT_URI
  },
};