import io
from PIL import Image, ImageDraw


def draw_rectangle_on_image(image_stream: io.BytesIO, coordinates: list[float]) -> io.BytesIO:
    image = Image.open(image_stream)
    draw = ImageDraw.Draw(image)
    draw.rectangle(coordinates, outline="red", width=2)
    new_image_stream = io.BytesIO()
    image.save(new_image_stream, format="jpeg")
    new_image_stream.seek(0)
    return new_image_stream
