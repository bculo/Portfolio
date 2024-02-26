from typing import Annotated

from fastapi import FastAPI, Depends
from faststream.rabbit import RabbitMessage
from faststream.rabbit.fastapi import RabbitRouter

from dependencies import di_blob_conf, di_blob_service, di_verification_service
from routes.message_broker.models import RabbitMqConfig
from services.BlobStorageService import BlobStorageService
from services.BlobStorageServiceConfig import BlobStorageServiceConfig
from services.NsfwService import UserVerificationService

conf = RabbitMqConfig()
router_broker = RabbitRouter(conf.conn_str)


@router_broker.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance successfully")


@router_broker.subscriber(conf.get_con("img_uploaded_con").queue,
                          conf.get_con("img_uploaded_con").intermediate_exc,
                          retry=False)
async def on_user_image_upload(message: RabbitMessage,
                               blob_service: Annotated[BlobStorageService, Depends(di_blob_service)],
                               blob_conf: Annotated[BlobStorageServiceConfig, Depends(di_blob_conf)],
                               verif_service: Annotated[UserVerificationService, Depends(di_verification_service)]):
    await verif_service.execute_user_verification_procedure(blob_service,
                                                            blob_conf.verification_cont(),
                                                            message,
                                                            conf,
                                                            router_broker)
