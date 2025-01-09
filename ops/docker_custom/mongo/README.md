### APPROACH 1

- navigate to ./ops/mango folder
- execute './generate_keyfile-local.sh'
- image should be mongo:7.0.4
- modify docker-compose file so that trend.mongo setup looks something like this

```
  ...

  trend.mongo:
    container_name: trend.mongo
    volumes:
      - ./data/mongo/trend:/data/db
      - ./ops/mongo/replica.key:/data/replica.key
    entrypoint:
      - bash
      - -c
      - |
        chmod 400 /data/replica.key
        chown 999:999 /data/replica.key
        exec docker-entrypoint.sh $$@
    command: "mongod --bind_ip_all --replSet myreplica --keyFile /data/replica.key"
    ports:
      - "27017:27017"
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: trendpassword

  ...
```

- run 'docker-compose up -d'
- setup up replica config
- execute 'docker exec -it trend.mongo /bin/sh'
- execute 'mongosh -u root -p trendpassword --host localhost --port 27017'
- execute this code in mongosh

```
rs.initiate({_id: 'myreplica', members: [{ _id: 0, host: 'localhost:27017' }]});
```

### APPROACH 2

- navigate to ./ops/mango folder
- execute 'docker build -t culix77/mongo-replica .'
- image should be culix77/mongo-replica
- modify docker-compose file so that trend.mongo setup looks something like this

```
  ...

  trend.mongo:
  container_name: trend.mongo
  volumes:
    - ./data/mongo/trend:/data/db
  command: "mongod --bind_ip_all --replSet myreplica --keyFile /opt/keyfile/mongo-keyfile"
  ports:
    - "27017:27017"
  environment:
    MONGO_INITDB_ROOT_USERNAME: root
    MONGO_INITDB_ROOT_PASSWORD: trendpassword

  ...
```

- run 'docker-compose up -d'
- setup up replica config
- execute 'docker exec -it trend.mongo /bin/sh'
- execute 'mongosh -u root -p trendpassword --host localhost --port 27017'
- execute this code in mongosh

```
rs.initiate({_id: 'myreplica', members: [{ _id: 0, host: 'localhost:27017' }]});
```

### TEST

- execute 'docker exec -it trend.mongo /bin/sh'
- execute 'mongosh -u root -p trendpassword --host myreplica/localhost --port 27017'
