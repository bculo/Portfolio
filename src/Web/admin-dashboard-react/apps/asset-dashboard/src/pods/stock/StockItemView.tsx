import { useParams } from 'react-router-dom';
import { myContainer } from '../../services/inversify.config';
import { WebSocketService } from '../../services/interfaces';
import { TYPES } from '../../services/types';
import { useEffect, useState } from 'react';
import { useGetStockQuery } from '../../stores/api/generated';
import { format } from 'date-fns';

const webSocketService = myContainer.get<WebSocketService>(
  TYPES.WebSocketService
);

export const StockItemView = () => {
  const params = useParams();

  const [price, setPrice] = useState<number | null>(null);

  const { data } = useGetStockQuery(params.id!);

  useEffect(() => {
    if (!data) return;
    webSocketService.joinGroup<{ symbol: string; price: number }>(
      data.symbol!,
      (response) => setPrice(response.price)
    );
    return () => webSocketService.leaveGroup(data.symbol!);
  }, [data]);

  const currentPrice = !price ? data?.price ?? 0 : price;

  return (
    <div className="grid grid-cols-3 gap-4">
      <div className="glass rounded-md p-4 flex justify-center items-center">
        <span>Symbol: </span>
        <span className="text-cyan-500 font-extrabold ml-2">
          {data?.symbol}
        </span>
      </div>

      <div className="glass rounded-md p-4 flex justify-center items-center">
        <span>Price: </span>
        <span className="text-cyan-500 font-extrabold ml-2">
          {currentPrice}
        </span>
      </div>

      <div className="glass rounded-md p-4 flex justify-center items-center">
        <span>Last update: </span>
        <span className="text-cyan-500 font-extrabold ml-2">
          {data?.lastPriceUpdate
            ? format(new Date(data.lastPriceUpdate), 'dd.MM.yyyy HH:mm')
            : ''}
        </span>
      </div>

      <div className="glass rounded-md p-4 flex justify-center items-center">
        <span>Created on: </span>
        <span className="text-cyan-500 font-extrabold ml-2">
          {data?.created
            ? format(new Date(data.created), 'dd.MM.yyyy HH:mm')
            : ''}
        </span>
      </div>

      <div className="glass rounded-md p-4 flex justify-center items-center">
        <span>Created on: </span>
        <span className="text-cyan-500 font-extrabold ml-2">
          {data?.isActive ? (
            <span className="text-green-600">Active</span>
          ) : (
            <span className="text-red-600">Unactive</span>
          )}
        </span>
      </div>
    </div>
  );
};
