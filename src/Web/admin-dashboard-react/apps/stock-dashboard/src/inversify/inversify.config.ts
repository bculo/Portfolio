import { Container } from "inversify";
import { TYPES } from "./types"
import { WebSocketService } from "./interfaces";
import { SignalRConnector } from "./services";

const myContainer = new Container();
myContainer.bind<WebSocketService>(TYPES.WebSocketService).to(SignalRConnector).inSingletonScope();

export { myContainer };

