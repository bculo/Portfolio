FROM mongo:7.0.4

COPY generate-keyfile-docker.sh ./generate-keyfile-docker.sh

RUN mkdir -p /opt/keyfile && /bin/bash ./generate-keyfile-docker.sh
