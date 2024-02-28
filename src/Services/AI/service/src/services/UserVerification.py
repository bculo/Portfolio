from typing_extensions import Annotated
from fastapi import Depends
from pydantic import BaseModel

from services.BlobStorage import BlobInfo, BlobStorageService, BlobStorageServiceConfig, di_blob_conf, di_blob_service
from utilities.face_detection_utilities import detect_faces, DetectedFace
from utilities.image_utilities import draw_rectangle_on_image
from utilities.nsfw_detection_utilites import detect_nsfw


class VerificationRequest(BaseModel):
    user_id: str
    blob_identifier: str
    user_name: str
    
class VerificationResult(BaseModel):
    is_person: bool
    is_nsfw: bool


class UserVerificationService:
    _blob_service: BlobStorageService
    _blob_config: BlobStorageServiceConfig
    
    def __init__(self, 
                 blob_service: BlobStorageService,
                 blob_config: BlobStorageServiceConfig) -> None:
        self._blob_service = blob_service
        self._blob_config = blob_config
    
    async def verify_user_image(self, verification_req: VerificationRequest) -> VerificationResult:
        blob_info = self._blob_service.download_blob(self._blob_config.verification_cont(), verification_req.blob_identifier)
        is_nsfw = detect_nsfw(blob_info.blob)
        detection_result, is_person = self.is_single_person(blob_info.blob)
        if is_person and not is_nsfw:
            self.create_image_with_face_coordinates(verification_req, blob_info, detection_result)
        return VerificationResult(is_person=is_person, is_nsfw=is_nsfw)

    def create_image_with_face_coordinates(self, verification_req: VerificationRequest, blob_info: BlobInfo, detection_result: DetectedFace):
        modified_blob = draw_rectangle_on_image(blob_info.blob, detection_result.coordinates)
        modified_blob_identifier = f"{verification_req.user_id}-detection"
        self._blob_service.upload_blob(modified_blob, 
                                      self._blob_config.verification_cont(), 
                                      modified_blob_identifier, 
                                      blob_info.content_type)

    def is_single_person(self, blob_info: BlobInfo) -> (DetectedFace | None, bool):
        face_det_result = detect_faces(blob_info)
        if len(face_det_result) == 1:
            return face_det_result[0], True
        return None, False

def di_verification_service() -> UserVerificationService:
    return UserVerificationService(di_blob_service(), di_blob_conf())
