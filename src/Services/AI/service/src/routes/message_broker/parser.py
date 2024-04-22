from faststream.rabbit import RabbitMessage

from services.UserVerification import VerificationRequest
from services.MailClassification import MailClassificationRequest

class ImageVerificationRequestParser:
    
    def to_request(self, message: RabbitMessage) -> VerificationRequest:
        user_id = message.body['user_id']
        blob_identifier = message.body['image_name']
        user_name = message.body['user_name']
        return VerificationRequest(user_id=user_id, blob_identifier=blob_identifier, user_name=user_name)
    
    
class MailClassificationRequestParser:
    
    def to_request(self, message: RabbitMessage) -> MailClassificationRequest:       
        content = message.body['message']
        user_id = message.body['user_id']
        title = message.body['title']
        from_user = message.body['from']
        return MailClassificationRequest(text=content, from_user=from_user, title=title, user_id=user_id)