import React, { useState } from 'react';
import { format } from 'date-fns';
import {
  FilterStocksApiArg,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { FilterStockForm, StockOverviewFilter } from './StockOverviewFilter';
import { Table } from '../../components/Table';

const map = (form: StockFilter): FilterStocksApiArg => {
  return {
    ...form,
    'Symbol.Value': form.symbol,
    'ActivityStatus.Value': form.status,
  };
};

type StockFilter = FilterStockForm & {
  page: number;
  take: number;
};

const defaultVal: StockFilter = {
  status: 999,
  symbol: '',
  page: 1,
  take: 20,
};

const StockOverviewPage = () => {
  const [filter, setFilter] = useState<StockFilter>(defaultVal);
  const { data } = useFilterStocksQuery(map(filter));

  return (
    <div className="w-2/4 m-auto">
      <div className="glass p-8 rounded-lg">
        <StockOverviewFilter
          defaultValue={defaultVal}
          onSearch={(f) => setFilter({ ...defaultVal, ...f })}
        />
      </div>

      <div className="mt-4">
        <Table
          take={filter.take}
          page={filter.page}
          onPageChange={(page) =>
            setFilter((prev) => ({ ...prev, page: page }))
          }
          totalRecords={data?.totalCount ?? 0}
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
