### REGISTER K6 CLIENT ON KEYCLOAK SERVER

Screen 1:
ClientID: k6-client

Screen 2:
Client authentication: ON
Service accounts roles: CHECK

Screen 3:
Root and home URL: http://localhost/

### GENERATE K6 CLIENT

- visit swagger builder, copy swagger.json and generate openapi-yaml client
- download generated yaml and place is inside this folder
- docker pull openapitools/openapi-generator-cli
- spin up docker container openapitools/openapi-generator-cli with

```
docker run --rm -v ${PWD}:/local openapitools/openapi-generator-cli generate -i ./local/openapi.yaml -g k6 -o ./local/k6-openapi-generated/
```
