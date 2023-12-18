import io
import uuid
from faststream.rabbit import RabbitMessage, RabbitRouter

from routes.message_broker.models import UserImageVerifiedEvent, RabbitMqConfig
from services.BlobStorageService import BlobStorageService
from utilities.face_detection_utilities import detect_faces, DetectedFace
from utilities.image_utilities import draw_rectangle_on_image
from utilities.nsfw_detection_utilites import detect_nsfw


async def execute_user_verification_procedure(blob_service: BlobStorageService,
                                              blob_container: str,
                                              received_message: RabbitMessage,
                                              broker_config: RabbitMqConfig,
                                              router: RabbitRouter):
    user_id = get_msg_property(received_message, 'user_id')
    blob_identifier = get_msg_property(received_message, 'image_name')
    user_name = get_msg_property(received_message, 'user_name')
    blob_info = blob_service.download_blob(blob_container, blob_identifier)
    detection_result = validate_person(blob_info.blob)
    is_person = False if detection_result is None else True
    if is_person:
        modified_blob = draw_rectangle_on_image(blob_info.blob, detection_result.coordinates)
        modified_blob_identifier = f"{user_id}-detection"
        blob_service.upload_blob(modified_blob, blob_container, modified_blob_identifier, blob_info.content_type)
    is_nsfw = detect_nsfw(blob_info.blob)
    new_event = define_publish_event(is_person, user_id, user_name, is_nsfw)
    await publish_event(router, new_event, broker_config)


def get_msg_property(received_message: RabbitMessage, property_name: str) -> str:
    return received_message.body[property_name]


def define_publish_event(detection_result: bool, user_id: str,
                         user_name: str, is_nsfw: bool) -> UserImageVerifiedEvent:
    return UserImageVerifiedEvent(userName=user_name, userId=user_id, isPerson=detection_result, isNsfw=is_nsfw)


async def publish_event(router: RabbitRouter, event: UserImageVerifiedEvent, conf: RabbitMqConfig):
    relevant_config = conf.get_pub('img_verified_pub')
    await router.broker.publish(event,
                                exchange=relevant_config.exchange_name,
                                delivery_mode=conf.delivery_mode,
                                content_type=conf.content_type,
                                message_id=uuid.uuid4(),
                                correlation_id=uuid.uuid4())


def validate_person(blob_stream: io.BytesIO) -> DetectedFace | None:
    face_det_result = detect_faces(blob_stream)
    if len(face_det_result) == 1:
        return face_det_result[0]
    return None
