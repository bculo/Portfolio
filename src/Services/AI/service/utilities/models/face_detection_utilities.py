import io

from huggingface_hub import hf_hub_download
from pydantic import BaseModel
from ultralytics import YOLO
from PIL import Image


_model_path = hf_hub_download(repo_id="arnabdhar/YOLOv8-Face-Detection", filename="model.pt")
_model = YOLO(_model_path)


class DetectedFace(BaseModel):
    coordinates: list[float] = []
    score: float


def detect_faces(image_bytes: bytes) -> list[DetectedFace]:
    io_bytes = io.BytesIO(image_bytes)
    image_file = Image.open(io_bytes)
    detections = _model(image_file, conf=.5)[0]

    detected_faces: list[DetectedFace] = []
    for detection in detections.boxes.data.tolist():
        x1, y1, x2, y2, score, label_id = detection
        score = round(score, 2)
        coordinates = [round(x1, 2), round(y1, 2), round(x2, 2), round(y2, 2)]
        detected_faces.append(DetectedFace(coordinates=coordinates, score=score))

    return detected_faces
