import io

from faststream.rabbit import RabbitMessage, RabbitRouter

from routes.message_broker.models import UserImageVerifiedEvent, MassTransitMessage, RabbitMqConfig
from services.BlobStorageService import BlobStorageService
from utilities.face_detection_utilities import detect_faces


async def execute_user_verification_procedure(blob_service: BlobStorageService,
                                              blob_container: str,
                                              received_message: RabbitMessage,
                                              broker_config: RabbitMqConfig,
                                              router: RabbitRouter):
    blob_identifier = get_blob_identifier(received_message)
    blob_stream = blob_service.download_blob(blob_container, blob_identifier)
    is_person = validate_person(blob_stream)
    user_id = extract_user_identifier(received_message)
    new_event = define_publish_event(is_person, user_id, broker_config)
    await publish_event(router, new_event, broker_config)


def get_blob_identifier(received_message: RabbitMessage) -> str:
    return received_message.body['image_name']


def extract_user_identifier(received_message: RabbitMessage) -> str:
    return received_message.body['user_id']


def define_publish_event(detection_result: bool, user_id: str, conf: RabbitMqConfig) -> MassTransitMessage:
    relevant_config = conf.get_pub('img_verified_pub')
    message_body = UserImageVerifiedEvent(userId=user_id, isPerson=detection_result)
    message_type = [relevant_config.masstransit_type]
    return MassTransitMessage[UserImageVerifiedEvent](message=message_body,
                                                      messageType=message_type)


async def publish_event(router: RabbitRouter, event: MassTransitMessage, conf: RabbitMqConfig):
    relevant_config = conf.get_pub('img_verified_pub')
    await router.broker.publish(event,
                                exchange=relevant_config.exchange_name,
                                delivery_mode=conf.delivery_mode,
                                content_type=conf.content_type,
                                message_id=event.messageId,
                                correlation_id=event.correlationId)


def validate_person(blob_stream: io.BytesIO):
    face_det_result = detect_faces(blob_stream)
    if len(face_det_result) != 1:
        return False
    return True
