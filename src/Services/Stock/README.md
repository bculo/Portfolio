### INFO

Stock.API is application responsible for managing stock items and prices

### What can you see inside this microservice ?
- CQRS pattern 
- Output caching and fussion cache usage (Redis)
- RabbitMQ / MassTransit usage
- PostgreSQL
- Hangfire for recurring background jobs

### Create Stock.API client for swagger OAuth2 implicit flow

- visit keycloak admin dashboard (http://localhost:8080). Should be up and running after docker-compose up -d
- select option 'Clients' from main menu and click "Create client" button
- set Client ID to Stock.API
- Enable 'Implicit flow'
- Set Root/Home page URL. For example http://localhost:32034/*


### Prepare database
- go to Stock.Infrastructure project and find migrations
- execute migrations

