from fastapi import FastAPI
from routes.message_broker import broker as msg_broker_endpoint
from routes.ai_endpoint import router as ai_endpoint

app = FastAPI(lifespan=msg_broker_endpoint.router_broker.lifespan_context)

app.include_router(msg_broker_endpoint.router_broker)
app.include_router(ai_endpoint.router)


@app.get("/")
async def root():
    return "OK"

