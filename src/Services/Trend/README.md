Trend project is fetching latest news about economy, crypto and stocks

### PREPARE MONGODB DATABASE FOR SERILOG

- Open terminal/cmd window
- execute 'docker exec -it trend_mongo_db /bin/sh'
- execute 'mongosh -u root -p trendpassword --host localhost --port 27017'
- execute 'use admin'
- execute 'db.createUser({user: 'logger', pwd: 'loggerpassword', roles: [ { role: 'readWrite', db: 'TrendLogs' } ] })'
- execute 'db.system.users.find().pretty()'


```
 {
    _id: 'adming.logger',
    userId: UUID("ebb59b9f-f579-4707-875f-d16333ee0bab"),
    user: 'logger',
    db: 'adming',
    credentials: {
      'SCRAM-SHA-1': {
        iterationCount: 10000,
        salt: 'Z7hLsUdc39GBGjRTwwvU9A==',
        storedKey: 'ZWXUad7giMWe86TRnzo/PUF1jjI=',
        serverKey: 'C4U8ZP9SJxrTc+iUgVDHb0XgJVg='
      },
      'SCRAM-SHA-256': {
        iterationCount: 15000,
        salt: 'tsgFmunOQ2K1PgiYyRXqaTiL2NwuClv5Bin1OA==',
        storedKey: 'HC/qc5F1/GP3xuEzV/mCf6GzhSuYtF5x7Q8S2OmjJrs=',
        serverKey: 'tQhzPhRBYCQEpnUAl4znhMJb9zDIjBcPemUyr+9SD0g='
      }
    },
    roles: [ { role: 'readWrite', db: 'TrendLogs' } ]
  }
```

- map properties to appsettings.json and appsettings.Development.json in Trend.API and Trend.GRPC projects and Trend.BackgroundSync


### PREPARE PROGRAMMABLE SEARCH ENGINE

- visit https://developers.google.com/custom-search/v1/overview?hl=en and select JSON API section overview
- Follow instructions and create search engine and claim API key and Engine ID
- Fill GoogleSearchOptions in appsettings.json files

### PREPARE AUTH

- visit keycloak admin dashboard (http://localhost:8080). Should be up and running after docker-compose up -d
- select option 'Clients' from main menu and click "Create client" button
- set Client ID to Trend.API
- Enable Client authentication and flows 'Service Accounts' roles and 'Implicit flow'
- Set Root/Home page URL. For example http://localhost:5276/

### EXAMPLE OF APPSETTINGS.JSON

```
{
  "RedisOptions": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "TrendRedis",
    "RememberTime": 30
  },
  "MongoOptions": {
    "ConnectionString": "mongodb://root:trendpassword@localhost:27017/?connectTimeoutMS=300000&authSource=admin",
    "DatabaseName": "Trend",
    "ConnectionTimeoutSeconds": 30,
    "UseInterceptor": false,
    "InterceptorSettings": {
      "User": "root",
      "Password": "trendpassword",
      "Port": 27017,
      "Host": "localhost",
      "AuthMechanisam": "SCRAM-SHA-1",
      "AuthDatabase": "admin"
    }
  },
  "GoogleSearchOptions": {
    "Uri": "https://www.googleapis.com/customsearch/v1",
    "ApiKey": "<your_api_key>",
    "SearchEngineId": "<your_search_engine_id>"
  },
  "SyncBackgroundServiceOptions": {
    "SleepTimeMiliseconds": 300000,
    "TimeSpanBetweenSyncsHours": 12
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Information",
        "Microsoft.AspNetCore": "Warning",
        "MongoDB.Driver": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" }
    ]
  },
  "SerilogMongo": {
    "UseLogger": true,
    "AuthMechanisam": "SCRAM-SHA-1",
    "AuthDatabase": "admin",
    "UserName": "logger",
    "Password": "loggerpassword",
    "BatchPostingLimit": 1,
    "Database": "TrendLogs",
    "Host": "localhost",
    "Port": 27017
  },
  "AuthOptions": {
    "PublicRsaKey": "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1I/GJkuAkASbVGCo9gp48OB3QvA+OZS6Jt7nwjNHe3D63QyAE3Y3oRvi++lvB3l4dW9fzRIMQkLc4sbGwB2fzKqJS3/hxszki3teUZcAq4EaUA6QhW85/NY5aW6vG3/pH4S/mhRzuPeE5t5/KDeBPnPfop15Y+CXHZkrogdYSNWzMKF66RbsBazVBi/+Fq41zOl51+pjq7fXGp3QbJt35qWJ9M1fxbNx2ZHkG+j4CJiUmyHErtnjmxCAtFJ9Uj57ygdRR2VyiST4Z6SQ8ovq0A6v1jxCSFFj6mRe9F/5cSd4dwyaAFq8qSn4OGEp0Q/0yfHMy6wAxDNy0JZquNCydwIDAQAB",
    "ValidIssuer": "http://localhost:8080/realms/PortfolioRealm",
    "ValidateAudience": false,
    "ValidateIssuer": true,
    "ValidateIssuerSigningKey": true,
    "ValidateLifetime": true
  },
  "KeycloakOptions": {
    "ApplicationName": "Trend.API",
    "RealmName": "PortfolioRealm",
    "AuthorizationServerUrl": "http://localhost:8080/realms/PortfolioRealm"
  },
  "ApiVersion": {
    "MajorVersion": 1,
    "MinorVersion": 1
  },
  "QueueOptions": {
    "Address": "amqp://rabbitmquser:rabbitmqpassword@localhost:5672"
  }
}
```