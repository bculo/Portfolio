import React, { useState } from 'react';
import { format } from 'date-fns';
import {
  FilterStocksApiArg,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { FilterStockForm, StockOverviewFilter } from './StockOverviewFilter';
import { Table } from '../../components/Table';

const map = (form: FilterStockForm): FilterStocksApiArg => {
  return {
    ...form,
    'Symbol.Value': form.symbol,
    'ActivityStatus.Value': form.status,
  };
};

const defaultFormValue: FilterStockForm = {
  status: 999,
  symbol: '',
  page: 1,
  take: 20,
};
//const something: FilterStockResponseItem;

const StockOverviewPage = () => {
  const [filter, setFilter] = useState<FilterStockForm>(defaultFormValue);
  const { isLoading, data } = useFilterStocksQuery(map(filter));

  return (
    <div className="w-2/4 m-auto">
      <div className="glass p-8 rounded-lg">
        <StockOverviewFilter
          defaultValue={defaultFormValue}
          onSearch={(f) => setFilter(f)}
        />
      </div>

      <div className="mt-4">
        <Table
          payload={data?.items ?? []}
          columns={{
            symbol: {
              name: 'Symbol',
              accessor: (item) => item.symbol,
            },
            lastPriceUpdate: {
              name: 'Last update',
              accessor: (item) => {
                return item.lastPriceUpdate
                  ? format(new Date(item.lastPriceUpdate), 'dd.MM.yyyy')
                  : '';
              },
            },
            price: { name: 'Price', accessor: (item) => item.price },
          }}
        />
      </div>
    </div>
  );
};

export default StockOverviewPage;
