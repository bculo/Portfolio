### PREPARE KEY AUTHENTICATION ?

- visit keycloak admin dashboard (http://localhost:8080). Should be up and running after docker-compose up -d
- select option 'Clients' from main menu and click "Create client" button
- set Client ID to Stock.Client
- set valid redirect URI
- go to advanced tab
- set "Proof Key for Code Exchange Code Challenge Method" to S256
- download adapter config

```json
{
  "realm": "PortfolioRealm",
  "auth-server-url": "http://localhost:8080/",
  "ssl-required": "external",
  "resource": "Stock.Client",
  "public-client": true,
  "confidential-port": 0
}
```

### HOW TO RUN IT?

```
nx serve stock-dashboard
```
