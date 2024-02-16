import React, { useState } from 'react';
import {
  FilterStocksApiArg,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { FilterStockForm, StockOverviewFilter } from './StockOverviewFilter';

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
      <p>Total count: {data?.totalCount}</p>
      {data?.items?.map((value, index) => (
        <div key={value.id}>{value.symbol}</div>
      ))}
    </div>
  );
};

export default StockOverviewPage;
