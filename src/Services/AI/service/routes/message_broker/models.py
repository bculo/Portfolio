from  datetime import datetime
import uuid
from typing import TypeVar, Generic
from uuid import UUID

from faststream.rabbit import RabbitExchange, RabbitQueue, ExchangeType
from pydantic import BaseModel


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
    userId: int
    isPerson: bool


class RabbitMqEndpoint:
    root_exc: RabbitExchange
    intermediate_exc: RabbitExchange
    queue: RabbitQueue

    def __init__(self, conf: any):

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




