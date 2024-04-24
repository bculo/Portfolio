export const environment = {
    stockApiBase: 'http://localhost:32034',
    websocket: 'http://localhost:5162/portfolio',
    oAuth2Config: {
        authority: 'http://localhost:8080/realms/PortfolioRealm/',
        client_id: 'Stock.Client',
        redirect_uri: 'http://localhost:4200/',
    }
}