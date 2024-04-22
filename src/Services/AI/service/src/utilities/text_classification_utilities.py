import torch
from transformers import pipeline

_DEVICE = torch.device("cuda" if torch.cuda.is_available() else "cpu")


_start_classification_model = "nlptown/bert-base-multilingual-uncased-sentiment"
_star_mapping = {
    "5 stars": 5,
    "4 stars": 4,
    "3 stars": 3,
    "2 stars": 2,
    "1 stars": 1,
}


class StartClassificationUtilityResult:
    
    def __init__(self, label_val: int, label_name: str, score: float) -> None:
        self.label_val = label_val
        self.label_name = label_name
        self.score = score
        

def get_star_classification(text: str) -> StartClassificationUtilityResult:
    model = pipeline(model=_start_classification_model, device=_DEVICE)
    result = model(text)[0]
    return StartClassificationUtilityResult(_star_mapping[result['label']],
                                      result['label'],
                                      result['score'])



