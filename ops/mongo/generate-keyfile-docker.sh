openssl rand -base64 756 > /opt/keyfile/mongo-keyfile

# Set the keyfile permissions to 400 and change the owner
chmod 400 /opt/keyfile/mongo-keyfile
chown 999:999 /opt/keyfile/mongo-keyfile