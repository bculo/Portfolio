import os
from  datetime import datetime
import uuid
from typing import TypeVar, Generic
from uuid import UUID

from faststream.rabbit import RabbitExchange, RabbitQueue, ExchangeType
from pydantic import BaseModel

from utilities.config_reader_utilities import read_yaml_file

DataT = TypeVar('DataT')


class MassTransitMessage(BaseModel, Generic[DataT]):
    messageId: UUID | None = uuid.uuid4()
    requestId: UUID | None = None
    correlationId: UUID | None = uuid.uuid4()
    conversationId: UUID | None = uuid.uuid4()
    initiatorId: UUID | None = None
    sourceAddress: str | None = None
    faultAddress: str | None = None
    message: DataT | None = None
    messageType: list[str] = []
    expirationTime: datetime | None = None
    sentTime: datetime = datetime.now()
    headers: dict = {}
    host: dict = {}


class UserImageVerifiedEvent(BaseModel):
    userId: str
    isPerson: bool


class RabbitMqConsumer:
    root_exc: RabbitExchange
    intermediate_exc: RabbitExchange
    queue: RabbitQueue

    def __init__(self, conf: dict):

        self.root_exc = RabbitExchange(conf["root_exchange"]["name"],
                                       auto_delete=False,
                                       type=ExchangeType.FANOUT,
                                       durable=True)

        self.intermediate_exc = RabbitExchange(conf["intermediate_exchange"]["name"],
                                               auto_delete=False,
                                               type=ExchangeType.FANOUT,
                                               durable=True,
                                               bind_to=self.root_exc)

        self.queue = RabbitQueue(conf["queue"]["name"],
                                 durable=True,
                                 routing_key="",
                                 auto_delete=False)


class RabbitMqPublisher:
    exchange_name: str
    masstransit_type: str

    def __init__(self, conf: dict):
        self.exchange_name = conf['root_exchange_name']
        self.masstransit_type = conf['masstransit_type']


class RabbitMqConfig:

    def __init__(self):
        config = read_yaml_file(os.path.join("configs", "broker.yaml"))

        self.conn_str = config["connection_string"]
        self.delivery_mode = config["default_delivery_mode"]
        self.content_type = config["default_content_type"]

        self._consumer = {
            'img_uploaded_con': RabbitMqConsumer(config["img_uploaded_con"]),
        }

        self._publisher = {
            'img_verified_pub': RabbitMqPublisher(config["img_verified_pub"]),
        }

    def get_con(self, endpoint_name) -> RabbitMqConsumer:
        return self._consumer[endpoint_name]

    def get_pub(self, endpoint_name) -> RabbitMqPublisher:
        return self._publisher[endpoint_name]




