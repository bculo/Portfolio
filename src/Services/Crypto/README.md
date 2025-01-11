### INFO

Crypto.API is application responsible for managing crypto items and prices

### What can you see inside this microservice ?
- CQRS pattern
- Fussion cache usage (Redis)
- RabbitMQ / MassTransit usage
- PostgreSQL / TimeseriesDB
- Hangfire for recurring background jobs

### Configure keycloak integration for authentication (Automated approach using Ansible)

- make sure you are located inside ops/local/ansible-scripts/
- execute ansible-playbook keycloak_realm_setup.yml

### Configure keycloak integration for authentication (Manual approach)

- visit keycloak admin dashboard (http://localhost:8080). Should be up and running after docker-compose up -d
- select option 'Clients' from main menu and click "Create client" button
- set Client ID to Crypto.API
- Enable 'Implicit flow'
- Set Root/Home page URL. For example http://localhost:{port}/*
- Update appsettings.Development.json with public_key from keycloak