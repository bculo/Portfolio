### INFO

User.Functions is applications responsible for managing portfolio users. Whole microservice is based on Azure functions, mostly
Http trigger functions.

### What can you see inside this microservice ?

- communication with Keycloak admin API for user management
- RabbitMQ usage via MassTransit
- Azure Blob storage usage

### Install azure-functions-core-tools

#### MAC
- brew tap azure/functions
- brew install azure-functions-core-tools@4

#### OTHER OS
- check https://github.com/Azure/azure-functions-core-tools installation instructions 

### Create new client for managing users

- visit keycloak admin dashboard
- select master realm (Default one)
- create new client with name 'portfolio-admin'
- Check Client authentication and Service access grant
- visit Service account roles tab, select assign roles. Set filter to 'Filter by clients' and assign all PortfolioRealm roles (Save)


### Create User.Functions client for swagger OAuth2 implicit flow

- visit keycloak admin dashboard
- select PortfolioRealm
- create new client with name 'User.Functions'
- Uncheck everything except 'Implicit flow' and 'Standard Flow'
- Valid redirect URI 'http://localhost:7071/*'
- Backchannel logout URL 'http://localhost:7071/api/sso-logout'

### Execute database migrations

- set User.Functions as startup project
- run func start

### View and manage azurite/azure storage

- download Microsoft Azure Storage Explorer 'https://azure.microsoft.com/en-us/products/storage/storage-explorer'
- this projects uses default azure account 'devstoreaccount1'

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
  },
  "BlobStorageOptions": {
    "ConnectionString": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:1002/devstoreaccount1;",
    "VerificationContainerName": "verification",
    "ProfileContainerName": "profile"
  }
}
```

