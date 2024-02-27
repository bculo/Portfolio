import uuid

from typing_extensions import Annotated

from fastapi import FastAPI, Depends
from faststream.rabbit import RabbitMessage
from faststream.rabbit.fastapi import RabbitRouter

from routes.message_broker.models import RabbitMqConfig, UserImageVerifiedEvent
from services.UserVerification import UserVerificationService, VerificationRequest, di_verification_service

conf = RabbitMqConfig()
router_broker = RabbitRouter(conf.conn_str)


@router_broker.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance successfully")

@router_broker.subscriber(conf.get_con("img_uploaded_con").queue,
                          conf.get_con("img_uploaded_con").intermediate_exc,
                          retry=False)
async def on_user_image_upload(message: RabbitMessage, verif_service: Annotated[UserVerificationService, Depends(di_verification_service)]):
    user_id = message.body['user_id']
    blob_identifier = message.body['image_name']
    user_name = message.body['user_name']
    
    request = VerificationRequest(user_id=user_id, blob_identifier=blob_identifier, user_name=user_name)
    service_res = await verif_service.verify_user_image(request)
    
    event = UserImageVerifiedEvent(userName=user_name, userId=user_id, 
                                   isPerson=service_res.is_person, isNsfw=service_res.is_nsfw)
    
    relevant_config = conf.get_pub('img_verified_pub')
    await router_broker.broker.publish(event,
                                       exchange=relevant_config.exchange_name,
                                       delivery_mode=conf.delivery_mode,
                                       content_type=conf.content_type,
                                       message_id=uuid.uuid4(),
                                       correlation_id=uuid.uuid4())
    
