import json

from fastapi import FastAPI, APIRouter

from faststream.rabbit.fastapi import RabbitRouter
from faststream.rabbit.annotations import RabbitMessage
from faststream.rabbit import ExchangeType, RabbitExchange, RabbitQueue, RabbitBroker

from routes.message_broker import models

router_broker = RabbitRouter("amqp://rabbitmquser:rabbitmqpassword@localhost:5672")

router_api = APIRouter(
    prefix="/message-broker",
    tags=["Broker"]
)


image_root_exchange = RabbitExchange("Events.Common.User:UserImageUploaded",
                                     auto_delete=False,
                                     type=ExchangeType.FANOUT,
                                     durable=True)

image_exchange = RabbitExchange("user-image-uploaded",
                                auto_delete=False,
                                type=ExchangeType.FANOUT,
                                durable=True,
                                bind_to=image_root_exchange)

image_queue = RabbitQueue("user-image-uploaded",
                          durable=True,
                          routing_key="",
                          auto_delete=False)


@router_broker.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance")


@router_api.get("/image_upload/", description="Do binary classification on given text")
async def publish_image_upload():
    message = RabbitMessage(body={"MESSAGE": "HELLO"}, raw_message={"MESSAGE": "HELLO"})
    await router_broker.broker.publish(message,
                                       exchange=image_exchange,
                                       queue=image_queue,
                                       content_type="application/vnd.masstransit+json",
                                       delivery_mode=2)


@router_broker.subscriber(image_queue, image_exchange, retry=False)
async def handle(message: RabbitMessage):
    print(message.body)
    print("HELLO WORLD")



