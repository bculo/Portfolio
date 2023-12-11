from fastapi import FastAPI

from pydantic import ValidationError
from faststream.rabbit.fastapi import RabbitRouter
from faststream.rabbit.annotations import RabbitMessage
from faststream.rabbit import ExchangeType, RabbitExchange, RabbitQueue, RabbitBroker

router = RabbitRouter("amqp://rabbitmquser:rabbitmqpassword@localhost:5672")

print(router.broker.apply_types)

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


@router.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance")


@router.subscriber(image_queue, image_exchange, retry=False)
async def handle(body: str, msg: RabbitMessage):
    print(msg)


