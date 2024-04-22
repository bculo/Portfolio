### HOW TO RUN IT?

- install aws cli locally -> https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html
- run docker-compose up -d command (needed for local dynamodb)
- configure aws user profile. Open powershell and execute command aws configure. Fill access key, secret and region data
```
   Example:
   AccessKey: test
   SecretKey: test
   Region: eu-central-1
```

- create "MailTemplate" table (category is partition key and Name is range/sort key)

```
aws dynamodb create-table --table-name MailTemplate --attribute-definitions AttributeName=Category,AttributeType=N  AttributeName=Name,AttributeType=S --key-schema AttributeName=Category,KeyType=HASH AttributeName=Name,KeyType=RANGE --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 --endpoint-url http://localhost:8000
```

- create "Mail" table

```
aws dynamodb create-table --table-name Mail --attribute-definitions AttributeName=UserId,AttributeType=S  AttributeName=Id,AttributeType=S --key-schema AttributeName=UserId,KeyType=HASH AttributeName=Id,KeyType=RANGE  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 --endpoint-url http://localhost:8000
```

- check if table are successfully created 

```
aws dynamodb list-tables --endpoint-url http://localhost:8000
```