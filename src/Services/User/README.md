### Install azure-functions-core-tools

#### MAC
- brew tap azure/functions
- brew install azure-functions-core-tools@4

#### OTHER OS
- check https://github.com/Azure/azure-functions-core-tools installation instructions 


### Register admin-client

- visit keycloak admin dashboard
- select master realm (Default one)

#### Create new client

- create new client with name
  - client identifier 'portfolio-admin'
  - check 'Direct access grants'
  - As URL use 'http://localhost/'

#### Create new realm user

- create user 'portfolio-admin-user'
- set email, first name, last name and password (Save)
- set email verified to 'True' (Save)
- Visit 'Credentials' tab and set password. Uncheck temporary password! (Save)
- visit Role mapping tab, select assign roles. Set filter to 'Filter by clients' and assign PortfolioRealm roles (Save)


### Execute database migrations

- set User.Functions as startup project
- execute migrations on database (update-database)

### Example of appsettings.json

```
{
  "JwtValidationOptions": {
    "PublicRsaKey": "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkt4YgL5h6kZ7nRCzVYdanwo/Tx98pJiK20nrgNUG0A2P+zl7Vtq/zoKr6rv6ws6hypkMWXivGwSIT83gzINgoZ0Uo6cUkJIb9XV+rhiwCJtCMCstHrfwURv98B1FlJW0YduPPnQA7rD9+zUIGfcCVhQ1T8fj57xQfYTiz6ffFT8iOt4M9ayzXElNnIhutecraoBE32gAdG4+DCVOMXb/j4VfpPqpM480xaf9MmG4U9prvIEs4rOXVb94q2GauNrndbbFRdUxDHEkpO38HDmj85QosmFCuI6t4cZSIyQmjIFPyWv6ndi9kkMvknXwO+o92Ua103rCO3hbu4ydq+3GwwIDAQAB",
    "ValidIssuer": "http://localhost:8080/auth/realms/PortfolioRealm",
    "ValidateAudience": false,
    "ValidateIssuer": true,
    "ValidateIssuerSigningKey": true,
    "ValidateLifetime": true
  },
  "ConnectionStrings": {
    "UserDb": "Host=localhost;Port=5432;Database=User;User Id=postgres;Password=florijan;"
  },
  "KeycloakOptions": {
    "AdminEndpoint": "http://localhost:8080/auth/realms/master/"
  },
  "KeycloakAdminOptions": {
    "TokenBaseUri": "http://localhost:8080/auth/realms/master/",
    "AdminApiBaseUri": "http://localhost:8080/auth/admin/realms/",
    "ClientId": "portfolio-admin",
    "UserName": "portfolio-admin-user",
    "Password": "test",
    "Realm": "PortfolioRealm"
  },
  "QueueOptions": {
    "Address": "amqp://rabbitmquser:rabbitmqpassword@localhost:5672"
  }
}
```