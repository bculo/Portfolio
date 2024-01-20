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

### PREPARE SEARCH ENGINE

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
- Trend.Worker
- Trend.Grpc