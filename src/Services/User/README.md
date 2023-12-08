### Install azure-functions-core-tools

#### MAC
- brew tap azure/functions
- brew install azure-functions-core-tools@4

#### OTHER OS
- check https://github.com/Azure/azure-functions-core-tools installation instructions 

### Create user for PortfolioRealm realm management

- visit keycloak admin dashboard
- select master realm (Default one)

#### Create new client

- create new client with name
  - client identifier 'portfolio-admin'
  - Check Client authentication and Service access grant
  - As URL use 'http://localhost/'

#### Create new realm user

- create user 'portfolio-admin-user'
- set email, first name, last name and password (Save)
- set email verified to 'True' (Save)
- Visit 'Credentials' tab and set password. Uncheck temporary password! (Save)
- visit Service account roles tab, select assign roles. Set filter to 'Filter by clients' and assign all PortfolioRealm roles (Save)


### Execute database migrations

- set User.Functions as startup project
- execute migrations on database (update-database)

### Example of appsettings.json

```
{
  "JwtValidationOptions": {
      "PublicRsaKey": "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfM+RGYwUxYyPzgo+osnEn5SM/T2EoPgsKCLmcAntGc/xV4DbY9UnkGurZhaMfA8f7LikDkwdC4UqfiIdrtW3mKc9Eiwpa/JuTV3Kchi+Gn3ToHxA3hkoMArngbsoxsugtV3ikcrNCBb7nQTnKhywvF80hUW00L1et6T01NlEPaFP3QkfeCAefHusWzmZKyvwDnnuV7Q0lccgljrMCXxd0u8p0jb6Xrw2S3G18UuaOa70DLMOl590P2Dl57uCOX4F3MmNSDAMm53MiKfkB84UevFpVliKEktQitK1AIHGin9Ttv3pf2CZ5ctbJpqe6In6buSx6LaGQPKIushsKrzhwIDAQAB",
      "ValidIssuer": "http://localhost:8080/realms/PortfolioRealm",
      "ValidateAudience": false,
      "ValidateIssuer": true,
      "ValidateIssuerSigningKey": true,
      "ValidateLifetime": true
  },
  "ConnectionStrings": {
    "UserDb": "Host=localhost;Port=5433;Database=User;User Id=postgres;Password=florijan;"
  },
  "AuthOptions": {
    "AuthorizationUrl": "http://localhost:8080/realms/PortfolioRealm/protocol/openid-connect/auth",
    "TokenBaseUri": "http://localhost:8080/realms/master/",
    "AdminApiBaseUri": "http://localhost:8080/admin/realms/",
    "ClientId": "portfolio-admin",
    "ClientSecert": "<Client_Secret_KEycloak>",
    "Realm": "PortfolioRealm"
  },
  "QueueOptions": {
    "Address": "amqp://rabbitmquser:rabbitmqpassword@localhost:5672"
  }
}
```

