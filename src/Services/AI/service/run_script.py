import os

from azure.storage.blob import BlobServiceClient

from utilities.config_reader_utilities import read_yaml_file

broker_config = read_yaml_file(os.path.join("configs", "azure-blob.yaml"))

account_url = broker_config["connection_string"]

blob_service_client = BlobServiceClient.from_connection_string(
    conn_str=account_url,
)

# Create the container
containers = blob_service_client.list_containers()

for container in containers:
    print(container)