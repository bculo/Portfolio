import io

from PIL import Image
from transformers import pipeline

_task = "image-classification"
_model_identifier = "Falconsai/nsfw_image_detection"
_threshold = .65


def detect_nsfw(image_stream: io.BytesIO) -> bool:
    image = Image.open(image_stream)
    classifier = pipeline(_task, model=_model_identifier)
    result_array = classifier(image)
    for result in result_array:
        if result['label'] == 'nsfw' and result['score'] > _threshold:
            return True
    return False
