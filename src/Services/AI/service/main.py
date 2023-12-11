from fastapi import FastAPI
from routes.text_classification import endpoint as tc_endpoint
from routes.message_broker import endpoint as msg_broker_endpoint

app = FastAPI(lifespan=msg_broker_endpoint.router.lifespan_context)

app.include_router(tc_endpoint.router)
app.include_router(msg_broker_endpoint.router)


@app.get("/")
async def root():
    return "OK"

