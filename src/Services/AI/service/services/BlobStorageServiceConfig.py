class BlobStorageServiceConfig:

    def __init__(self, settings: dict):
        self._storage_config = settings

    def conn_str(self) -> str:
        return self._storage_config["connection_string"]

    def verification_cont(self) -> str:
        return self._storage_config["verification_container"]

    def profile_cont(self) -> str:
        return self._storage_config["profile_container"]

