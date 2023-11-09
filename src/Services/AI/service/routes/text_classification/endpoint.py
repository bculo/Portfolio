from typing import Annotated
from fastapi import APIRouter, Body

from routes.text_classification import models
from utilities.models.text_classification_utilities import get_star_classification

router = APIRouter(
    prefix="/text-classification",
    tags=["text classification"]
)


@router.post("/binary-sentiment/", 
             response_model=models.BinaryClassificationResponse,
             description="Do binary classification on given text")
async def binary_sentiment(request: Annotated[models.BinaryClassificationRequest, Body]):
    instance = models.BinaryClassificationResponse(score=1, label=2)
    return instance


@router.post("/star-sentiment/", 
             response_model=models.StartSentimentResponse,
             description="Do start sentiment on given text")
async def start_sentiment(request: Annotated[models.BinaryClassificationRequest, Body]):
    result = get_star_classification(request.text)
    return models.StartSentimentResponse(score=result.score, label_str=result.label_name, label_num=result.label_val)
    