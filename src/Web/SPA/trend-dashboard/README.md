### What is Trend Dashboard

Trend dashborad application is reponsible for communication with Trend.API. It helps users with 'Admin' role to mange news. For authentication it uses Keycloak.

### What can we see learn/see here?

- BEM methodology
- usage of ngrx for state managment
- communication with Trend.API microservice
- authentication via Keycloak

### Keycloak integration

- add client 'Trend.Client' to PortfolioRealm.
- check standard flow
- set valid redirect URI to 'http://localhost:4200/\*'
- navigate to Clients and select Trend.Client
- click on 'Action' select input and than click 'Download adapter config'
- copy config content to keycloak.json that is located inside assets folder (src/assets/keycloak.json)

### Keycloak admin

- make sure that you have 'Admin' user
- on keycloak admin dashboard select Users navigation item, then 'Add User' button
- set username to 'portfolio-admin' and fill up rest of the data. Make sure that created user is enabled.

### How to run it?

- make sure you are using node version ≈ 16.x.x. Easiest way to manage NODE version is with NVM tool.
- npm install
- ng serve -o
