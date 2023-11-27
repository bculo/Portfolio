#### Prepare image for keycloak MSSQL server combination

- execute 'docker build -t <your_tag> .' NOTE: this is DEV keycloak build and shouldn't be used in production environment

#### Spin up docker compose

- set appropriate image name for keycloak container in docker-compose.yaml
- run 'docker-compose up -d'
- make sure that Keycloak database is created and that everything is running fine

####

- visit admin dashboard 'http://localhost:8080'
- login as admin
- create new realm PortfolioRealm
