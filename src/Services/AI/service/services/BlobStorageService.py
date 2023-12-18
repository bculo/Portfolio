import io

from azure.storage.blob import BlobServiceClient, BlobClient

from services.BlobStorageServiceConfig import BlobStorageServiceConfig


class BlobStorageService:

    def __init__(self, conf: BlobStorageServiceConfig):
        self.conf = conf
        self._blob_service_client = BlobServiceClient.from_connection_string(
            conn_str=conf.conn_str(),
        )

    def _get_blob_client(self, container: str, identifier: str) -> BlobClient:
        return self._blob_service_client.get_container_client(container).get_blob_client(identifier)

    def download_blob(self, container: str, blob: str) -> io.BytesIO():
        stream = io.BytesIO()
        blob_client = self._get_blob_client(container, blob)
        blob_client.download_blob().readinto(stream)
        stream.seek(0)
        return stream

