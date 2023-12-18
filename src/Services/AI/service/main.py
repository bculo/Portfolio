from fastapi import FastAPI
from routes.message_broker import setup as msg_broker_endpoint

app = FastAPI(lifespan=msg_broker_endpoint.router_broker.lifespan_context)

app.include_router(msg_broker_endpoint.router_broker)


@app.get("/")
async def root():
    return "OK"

