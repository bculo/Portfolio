from uuid import UUID
from pydantic import BaseModel
from datetime import datetime


class MassTransitMessage(BaseModel):
    message_id: UUID | None
    request_id: UUID | None
    correlation_id: UUID | None
    conversation_id: UUID | None
    initiator_id: UUID | None
    source_address: str | None
    destination_address: str | None
    message: str | None
