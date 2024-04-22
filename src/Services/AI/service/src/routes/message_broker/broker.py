import uuid

from typing_extensions import Annotated

from fastapi import FastAPI, Depends
from faststream.rabbit import RabbitMessage
from faststream.rabbit.fastapi import RabbitRouter

from routes.message_broker.models import MailSentimentCheckedEvent, RabbitMqConfig, UserImageVerifiedEvent
from services.UserVerification import UserVerificationService, di_verification_service
from services.MailClassification import di_mail_classification, MailClassificationService
from routes.message_broker.parser import ImageVerificationRequestParser, MailClassificationRequestParser
from faststream.rabbit.shared.schemas import RabbitExchange
from faststream.rabbit.shared.constants import ExchangeType

conf = RabbitMqConfig()
router_broker = RabbitRouter(conf.conn_str)

@router_broker.after_startup
async def test(app: FastAPI):    
    for publisher_config_key in conf.get_publisher_keys():
        publisher = conf.get_pub(publisher_config_key)
        exchange = RabbitExchange(publisher.exchange_name, type=ExchangeType.FANOUT, durable=True)
        await router_broker.broker.declare_exchange(exchange)    
    print("Connected to RabbitMQ instance successfully")

"""
_summary_
Listen for user profile image upload event
"""
@router_broker.subscriber(conf.get_con("img_uploaded_con").queue,
                          conf.get_con("img_uploaded_con").intermediate_exc,
                          retry=False)
async def on_user_image_upload(message: RabbitMessage, verif_service: Annotated[UserVerificationService, Depends(di_verification_service)]):
    request = ImageVerificationRequestParser().to_request(message)
    service_res = await verif_service.verify_user_image(request)   
    event = UserImageVerifiedEvent(userName=request.user_name, userId=request.user_id, 
                                   isPerson=service_res.is_person, isNsfw=service_res.is_nsfw)  
    relevant_config = conf.get_pub('img_verified_pub')
    await router_broker.broker.publish(event,
                                       exchange=relevant_config.exchange_name,
                                       delivery_mode=conf.delivery_mode,
                                       content_type=conf.content_type,
                                       message_id=uuid.uuid4(),
                                       correlation_id=message.correlation_id)

"""
_summary_
Listen for check mail sentiment event
"""
@router_broker.subscriber(conf.get_con("mail_classification_con").queue,
                          conf.get_con("mail_classification_con").intermediate_exc,
                          retry=False)
async def on_check_mail_sentiment(message: RabbitMessage, sentiment_service: Annotated[MailClassificationService, Depends(di_mail_classification)]):
    request = MailClassificationRequestParser().to_request(message)
    service_res = sentiment_service.text_classification(request)
    event = MailSentimentCheckedEvent(numberOfStars=5, score=5, fromMail=request.from_user,
                                      userId=request.user_id, title=request.title, content=request.text)
    relevant_config = conf.get_pub('mail_sentiment_checked_pub')
    print(relevant_config.exchange_name)
    await router_broker.broker.publish(event,
                                       exchange=relevant_config.exchange_name,
                                       delivery_mode=conf.delivery_mode,
                                       content_type=conf.content_type,
                                       message_id=uuid.uuid4(),
                                       correlation_id=uuid.uuid4())
    
