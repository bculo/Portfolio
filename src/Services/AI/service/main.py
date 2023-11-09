from fastapi import FastAPI

from routes.text_classification import endpoint as tc_endpoint
from utilities.models import text_classification_utilities

app = FastAPI()

app.include_router(tc_endpoint.router)

@app.get("/")
async def root():
    return {"message": "Hello World"}


text_classification_utilities.init()