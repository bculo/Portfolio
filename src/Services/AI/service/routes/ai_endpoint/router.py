from typing import Annotated
from fastapi import APIRouter, Body

from routes.ai_endpoint import schemas
from utilities.text_classification_utilities import get_star_classification

router = APIRouter(
    prefix="/ai",
    tags=["Classification"]
)


@router.post("/binary-sentiment/", 
             response_model=schemas.BinaryClassificationResponse,
             description="Do binary classification on given text")
async def binary_sentiment(request: Annotated[schemas.BinaryClassificationRequest, Body]):
    instance = schemas.BinaryClassificationResponse(score=1, label=2)
    return instance


@router.post("/star-sentiment/", 
             response_model=schemas.StartSentimentResponse,
             description="Do start sentiment on given text")
async def start_sentiment(request: Annotated[schemas.BinaryClassificationRequest, Body]):
    result = get_star_classification(request.text)
    return schemas.StartSentimentResponse(score=result.score, label_str=result.label_name, label_num=result.label_val)
    