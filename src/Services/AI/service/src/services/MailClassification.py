from pydantic import BaseModel
# from utilities.text_classification_utilities import get_star_classification, StartClassificationUtilityResult

class MailClassificationRequest(BaseModel):
    text: str
    user_id: str
    title: str
    from_user: str

class MailClassificationService:
    
    def text_classification(self, req: MailClassificationRequest):
        return { 'label_name': '5 start', 'score': 5 }

def di_mail_classification() -> MailClassificationService:
    return MailClassificationService()