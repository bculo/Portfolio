from fastapi import FastAPI, APIRouter

from faststream.rabbit.fastapi import RabbitRouter
from faststream.rabbit.annotations import RabbitMessage

from routes.message_broker import models
from utilities.helpers.config_reader import read_yaml_file


router_api = APIRouter(
    prefix="/message-broker",
    tags=["Broker"]
)

broker_config = read_yaml_file("broker.yaml")
router_broker = RabbitRouter(broker_config["connection_string"])

conf = broker_config["image_uploaded_instance"]
image_upload_instance = models.RabbitMqEndpoint(broker_config["image_uploaded_instance"])


@router_broker.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance")


@router_broker.subscriber(image_upload_instance.queue, image_upload_instance.intermediate_exc, retry=False)
async def on_user_image_upload(message: RabbitMessage):
    print(message.body)





