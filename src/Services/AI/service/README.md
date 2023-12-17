### INFO

AI service is used for text classification and image classification. It listens for RabbitMQ messages and react accordingly based on message type.

### What can you see inside this microservice ?

- faststream usage for RabbitMQ communcation (receiving and sending message)
- usage of AI models

### HOW TO RUN IT?

- install python
- pip install pipenv (only if not installed)
- pipenv install
- pipenv run uvicorn main:app --reload

NOTE: make sure that you are located inside AI/service folder when executing pipenv commands
NOTE: first application start could be slow, because models needs to be downloaded locally
