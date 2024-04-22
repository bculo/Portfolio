import os

from faststream.rabbit import RabbitExchange, RabbitQueue, ExchangeType
from pydantic import BaseModel

from utilities.config_reader_utilities import read_yaml_file


class BaseEvent(BaseModel):
    pass

class UserImageVerifiedEvent(BaseEvent):
    userId: str
    isPerson: bool
    isNsfw: bool
    userName: str
    
class MailSentimentCheckedEvent(BaseEvent):
    numberOfStars: int
    score: float
    fromMail: str
    userId: str
    title: str
    content: str

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
            'mail_classification_con': RabbitMqConsumer(config["mail_classification_con"]),
        }

        self._publisher = {
            'img_verified_pub': RabbitMqPublisher(config["img_verified_pub"]),
            'mail_sentiment_checked_pub': RabbitMqPublisher(config["mail_sentiment_checked_pub"]),
        }

    def get_publisher_keys(self):
        return self._publisher.keys()

    def get_con(self, endpoint_name) -> RabbitMqConsumer:
        return self._consumer[endpoint_name]

    def get_pub(self, endpoint_name) -> RabbitMqPublisher:
        return self._publisher[endpoint_name]




