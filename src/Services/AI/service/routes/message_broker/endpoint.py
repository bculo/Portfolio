from fastapi import FastAPI, APIRouter

from faststream.rabbit.fastapi import RabbitRouter
from faststream.rabbit.annotations import RabbitMessage

from routes.message_broker import models
from routes.message_broker.models import MassTransitMessage, UserImageVerifiedEvent
from utilities.helpers.config_reader import read_yaml_file


router_api = APIRouter(
    prefix="/message-broker",
    tags=["Broker"]
)

broker_config = read_yaml_file("broker.yaml")
router_broker = RabbitRouter(broker_config["connection_string"])

image_upload_instance = models.RabbitMqEndpoint(broker_config["image_uploaded_instance"])


@router_broker.after_startup
async def test(app: FastAPI):
    print("Connected to RabbitMQ instance successfully")


@router_broker.subscriber(image_upload_instance.queue, image_upload_instance.intermediate_exc, retry=False)
async def on_user_image_upload(message: RabbitMessage):
    print(message.body)


@router_api.get("/send_message_to_dotnet/", description="Send message to dotnet consumer")
async def send_message_to_dotnet():
    conf = broker_config["image_verified_instance"]
    user_verified = UserImageVerifiedEvent(userId=1, isPerson=False)
    request = MassTransitMessage[UserImageVerifiedEvent](message=user_verified, messageType=[conf["masstransit_type"]])

    await router_broker.broker.publish(request,
                                       exchange=conf["root_exchange"]["name"],
                                       delivery_mode=broker_config["default_delivery_mode"],
                                       content_type=broker_config["default_content_type"],
                                       message_id=request.messageId,
                                       correlation_id=request.correlationId)
    return "WORKS"





