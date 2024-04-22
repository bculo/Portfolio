from pydantic import BaseModel
# from utilities.text_classification_utilities import get_star_classification, StartClassificationUtilityResult

class MailClassificationRequest(BaseModel):
    text: str
    user_id: str
    title: str
    from_user: str
    
class MailClassificationResponse(BaseModel):
    score: float
    num_of_starts: int

class MailClassificationService:
    
    def text_classification(self, req: MailClassificationRequest) -> MailClassificationResponse:  
        #return get_star_classification(req.text)   
        return MailClassificationResponse(score=5, num_of_starts=5)

def di_mail_classification() -> MailClassificationService:
    return MailClassificationService()