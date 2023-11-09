from pydantic import BaseModel, Field


class BinaryClassificationRequest(BaseModel):
    text: str = Field(default=None,  max_length=1500, min_length=5)
    
    model_config = {
        'json_schema_extra': {
            'examples': [
                {
                    "text": "This is great example."
                }
            ]
        }
    }
   
    
class BinaryClassificationResponse(BaseModel):
    score: float
    label: int
    
    
class StartSentimentRequest(BaseModel):
    text: str = Field(default=None,  max_length=1500, min_length=5)
    
    model_config = {
        'json_schema_extra': {
            'examples': [
                {
                    "text": "This is great example."
                }
            ]
        }
    }
    
    
class StartSentimentResponse(BaseModel):
    score: float
    label_str: str
    label_num: int