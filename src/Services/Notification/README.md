Notification.Hub microservice is responsible for receiving and processing events from other microservices via message broker.
When event is/are received, corresponding messages are created and pushed to connected client/clients via SignalR. It's relatively easy
to scale horizontally because Redis is used.  

### How to run it?
Project doesn't need any additional configuration. Just run it via Rider, Visual Studio or Terminal using command 'dotnet run'

### How to test it via Postman?
- add new request (websocket)
- connect to ws://localhost:5162/portfolio
- send message
```json
{
  "protocol": "json",
  "version": 1
}
```
- connect to group
```json
{
  "arguments": ["BTC"],
  "target": "JoinToGroup",
  "type": 1
}
```

### Example of appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AuthOptions": {
    "PublicRsaKey": "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfM+RGYwUxYyPzgo+osnEn5SM/T2EoPgsKCLmcAntGc/xV4DbY9UnkGurZhaMfA8f7LikDkwdC4UqfiIdrtW3mKc9Eiwpa/JuTV3Kchi+Gn3ToHxA3hkoMArngbsoxsugtV3ikcrNCBb7nQTnKhywvF80hUW00L1et6T01NlEPaFP3QkfeCAefHusWzmZKyvwDnnuV7Q0lccgljrMCXxd0u8p0jb6Xrw2S3G18UuaOa70DLMOl590P2Dl57uCOX4F3MmNSDAMm53MiKfkB84UevFpVliKEktQitK1AIHGin9Ttv3pf2CZ5ctbJpqe6In6buSx6LaGQPKIushsKrzhwIDAQAB",
    "ValidIssuer": "http://localhost:8080/realms/PortfolioRealm",
    "ValidateAudience": false,
    "ValidateIssuer": true,
    "ValidateIssuerSigningKey": true,
    "ValidateLifetime": true
  },
  "KeycloakOptions": {
    "ApplicationName": "Notification.HUB",
    "RealmName": "PortfolioRealm",
    "AuthorizationServerUrl": "http://localhost:8080/realms/PortfolioRealm"
  },
  "SignalROptions": { 
    "UseRedis": true,
    "AppPrefix": "signalr",
    "RedisConnection": "127.0.0.1:6379"
  },
  "QueueOptions": {
    "Address": "amqp://rabbitmquser:rabbitmqpassword@localhost:5672"
  }
}
 
```