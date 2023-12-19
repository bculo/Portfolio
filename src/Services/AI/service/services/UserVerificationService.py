import uuid
from faststream.rabbit import RabbitMessage, RabbitRouter
from pydantic import BaseModel

from routes.message_broker.models import UserImageVerifiedEvent, RabbitMqConfig
from services.BlobStorageService import BlobStorageService, BlobInfo
from utilities.face_detection_utilities import detect_faces, DetectedFace
from utilities.image_utilities import draw_rectangle_on_image
from utilities.nsfw_detection_utilites import detect_nsfw


class PropertyContainer(BaseModel):
    user_id: str
    blob_identifier: str
    user_name: str


class UserVerificationService:
    event: RabbitMessage
    props: PropertyContainer
    blob_info: BlobInfo

    async def execute_user_verification_procedure(self,
                                                  blob_service: BlobStorageService,
                                                  blob_container: str,
                                                  received_message: RabbitMessage,
                                                  broker_config: RabbitMqConfig,
                                                  router: RabbitRouter):
        self.extract_msg_properties(received_message)
        self.blob_info = blob_service.download_blob(blob_container, self.props.blob_identifier)
        is_nsfw = detect_nsfw(self.blob_info.blob)
        detection_result, is_person = self.is_single_person()
        if is_person and not is_nsfw:
            self.create_image_with_face_coordinates(blob_service, blob_container, detection_result)
        new_event = self.define_publish_event(is_person, is_nsfw)
        await self.publish_event(router, new_event, broker_config)

    def create_image_with_face_coordinates(self, blob_service: BlobStorageService,
                                           container: str, detection_result: DetectedFace):
        modified_blob = draw_rectangle_on_image(self.blob_info.blob, detection_result.coordinates)
        modified_blob_identifier = f"{self.props.user_id}-detection"
        blob_service.upload_blob(modified_blob, container, modified_blob_identifier, self.blob_info.content_type)

    def extract_msg_properties(self, received_message: RabbitMessage):
        self.event = received_message
        user_id = self.get_msg_property('user_id')
        blob_identifier = self.get_msg_property('image_name')
        user_name = self.get_msg_property('user_name')
        self.props = PropertyContainer(user_id=user_id, blob_identifier=blob_identifier, user_name=user_name)

    def get_msg_property(self, property_name: str) -> str:
        return self.event.body[property_name]

    def define_publish_event(self, detection_result: bool, is_nsfw: bool) -> UserImageVerifiedEvent:
        return UserImageVerifiedEvent(userName=self.props.user_name, userId=self.props.user_id,
                                      isPerson=detection_result, isNsfw=is_nsfw)

    async def publish_event(self, router: RabbitRouter, event: UserImageVerifiedEvent, conf: RabbitMqConfig):
        relevant_config = conf.get_pub('img_verified_pub')
        await router.broker.publish(event,
                                    exchange=relevant_config.exchange_name,
                                    delivery_mode=conf.delivery_mode,
                                    content_type=conf.content_type,
                                    message_id=uuid.uuid4(),
                                    correlation_id=uuid.uuid4())

    def is_single_person(self) -> (DetectedFace | None, bool):
        face_det_result = detect_faces(self.blob_info.blob)
        if len(face_det_result) == 1:
            return face_det_result[0], True
        return None, False
