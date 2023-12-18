import os

from services.BlobStorageService import BlobStorageService
from services.BlobStorageServiceConfig import BlobStorageServiceConfig
from utilities.config_reader_utilities import read_yaml_file

_blob_storage_conf_dict = read_yaml_file(os.path.join("configs", "azure-blob.yaml"))
_blob_storage_conf = BlobStorageServiceConfig(settings=_blob_storage_conf_dict)
_blob_storage_service = BlobStorageService(conf=_blob_storage_conf)


def di_blob_service() -> BlobStorageService:
    return _blob_storage_service


def di_blob_conf() -> BlobStorageServiceConfig:
    return _blob_storage_conf
