import { Container } from 'inversify';
import { TYPES } from './types';
import { WebSocketService } from './interfaces';
import { SignalRConnector } from './services';

const container = new Container();

container
  .bind<WebSocketService>(TYPES.WebSocketService)
  .to(SignalRConnector)
  .inSingletonScope();

export { container as myContainer };
