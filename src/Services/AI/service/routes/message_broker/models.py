from uuid import UUID

from faststream.rabbit import RabbitExchange, RabbitQueue, ExchangeType
from pydantic import BaseModel


class MassTransitMessage(BaseModel):
    message_id: UUID | None
    request_id: UUID | None
    correlation_id: UUID | None
    conversation_id: UUID | None
    initiator_id: UUID | None
    source_address: str | None
    destination_address: str | None
    message: str | None


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




