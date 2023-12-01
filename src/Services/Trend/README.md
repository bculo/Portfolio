Trend project is responsible for fetching latest news about economy, crypto, stocks etc.

### CHECK IF MONGO REPLICA IS CONFIGURED

- execute 'docker exec -it trend.mongo /bin/sh'
- execute 'mongosh -u root -p trendpassword --host localhost --port 27017'
- check if replica config is set

```
rs.status()
```

- if not set, execute this code to set replica config

```
rs.initiate({
    _id: 'myreplica',
    members: [
        { _id: 0, host: 'localhost:27017' }
    ]
});
```

### PREPARE MONGODB DATABASE FOR SERILOG

- Open terminal/cmd window
- execute 'docker exec -it trend.mongo /bin/sh'
- execute 'mongosh -u root -p trendpassword --host myreplica/localhost --port 27017'
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


### Runnable projects

- Trend.API
- Trend.BackgroundSync
- Trend.Grpc