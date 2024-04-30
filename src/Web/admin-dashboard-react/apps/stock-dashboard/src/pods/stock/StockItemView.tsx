import { useParams } from 'react-router-dom';
import { myContainer } from '../../inversify/inversify.config';
import { WebSocketService } from '../../inversify/interfaces';
import { TYPES } from '../../inversify/types';
import { useEffect } from 'react';

const webSocketService = myContainer.get<WebSocketService>(
  TYPES.WebSocketService
);

export const StockItemView = () => {
  const params = useParams();

  useEffect(() => {
    webSocketService.joinGroup<unknown>(params.symbol!, (response) =>
      console.log(response)
    );
    return () => webSocketService.leaveGroup(params.symbol!);
  }, [params]);

  return <div>StockItemView</div>;
};
