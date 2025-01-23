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
- execute ansible-playbook rabbitmq_setup.yml
- execute ansible-playbook appsettings_setup.yml

