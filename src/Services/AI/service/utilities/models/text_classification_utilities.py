import torch
from transformers import pipeline

_DEVICE = torch.device("cuda" if torch.cuda.is_available() else "cpu")


def init():
    print(f"{__file__} initialized")


_STAR_CLASSIFICATION_MODEL = "nlptown/bert-base-multilingual-uncased-sentiment"
_STAR_MAPPING = {
    "5 stars": 5,
    "4 stars": 4,
    "3 stars": 3,
    "2 stars": 2,
    "1 stars": 1,
}


class StartClassificationUtility:
    
    def __init__(self, label_val: int, label_name: str, score: float) -> None:
        self.label_val=label_val
        self.label_name=label_name
        self.score=score
        

def get_star_classification(text: str) -> StartClassificationUtility:
    model = pipeline(model=_STAR_CLASSIFICATION_MODEL, device=_DEVICE)
    result = model(text)[0]
    return StartClassificationUtility(_STAR_MAPPING[result['label']],
                                      result['label'],
                                      result['score'])



