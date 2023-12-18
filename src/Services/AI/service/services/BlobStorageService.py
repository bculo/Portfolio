import io

from azure.storage.blob import BlobServiceClient, BlobClient, ContentSettings, BlobType

from services.BlobStorageServiceConfig import BlobStorageServiceConfig


class BlobInfo:

    def __init__(self, blob: io.BytesIO, content_type: str):
        self.blob = blob
        self.content_type = content_type


class BlobStorageService:

    def __init__(self, conf: BlobStorageServiceConfig):
        self.conf = conf
        self._blob_service_client = BlobServiceClient.from_connection_string(
            conn_str=conf.conn_str(),
        )

    def _get_blob_client(self, container: str, identifier: str) -> BlobClient:
        return self._blob_service_client.get_container_client(container).get_blob_client(identifier)

    def download_blob(self, container: str, blob_identifier: str) -> BlobInfo:
        stream = io.BytesIO()
        blob_client = self._get_blob_client(container, blob_identifier)
        blob_client.download_blob().readinto(stream)
        stream.seek(0)
        content_type = blob_client.get_blob_properties().content_settings.content_type
        return BlobInfo(stream, content_type)

    def upload_blob(self, stream: io.BytesIO, container: str, blob_identifier: str, content_type: str):
        content_settings = ContentSettings(content_type=content_type)
        blob_client = self._get_blob_client(container, blob_identifier)
        blob_client.upload_blob(stream, blob_type=BlobType.BLOCKBLOB, overwrite=True)
        blob_client.set_http_headers(content_settings=content_settings)

