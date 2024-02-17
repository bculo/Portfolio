import React, { useState } from 'react';
import { format } from 'date-fns';
import {
  FilterStocksApiArg,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { FilterStockForm, StockOverviewFilter } from './StockOverviewFilter';
import { Table } from '../../components/Table';
import { Button } from '../../components/Button';
import { Modal } from '../../components/Modal';
import { CreateStockForm } from './CreateStockForm';

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
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [filter, setFilter] = useState<StockFilter>(defaultVal);
  const { data, refetch } = useFilterStocksQuery(map(filter));

  return (
    <div className="w-2/4 m-auto">
      <div className="flex justify-end gap-x-2 mb-4">
        <Button
          type="button"
          buttonStyle="full"
          text="Create"
          onClick={() => setIsModalOpen(true)}
        />
      </div>
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

      <Modal
        onModalClose={() => setIsModalOpen(false)}
        title="Create stock item"
        isOpen={isModalOpen}
      >
        <CreateStockForm
          onCreated={() => {
            setIsModalOpen(false);
            refetch();
          }}
        />
      </Modal>
    </div>
  );
};

export default StockOverviewPage;
