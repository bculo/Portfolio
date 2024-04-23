import { useCallback, useState } from 'react';
import { format } from 'date-fns';
import {
  FilterStocksApiArg,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { Table } from '../../components/Table';
import { Button } from '../../components/Button';
import { Modal } from '../../components/Modal';
import { CreateStockForm } from './CreateStockForm';
import { SearchInput } from '../../components/SearchInput';

const map = (form: StockFilter): FilterStocksApiArg => {
  return {
    ...form,
    'Symbol.Value': form.symbol,
    'ActivityStatus.Value': form.status,
  };
};

type StockFilter = {
  page: number;
  take: number;
  symbol: string;
  status: 1 | 2 | 999;
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

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search }));
  }, []);

  return (
    <div className="w-3/4 m-auto glass p-4 rounded-md">
      <div className="flex justify-between items-center">
        <div>
          <SearchInput
            placeholder="Search by symbol"
            onInputChange={onSearchChange}
          />
        </div>
        <div>
          <Button
            type="button"
            buttonStyle="full"
            text="Create"
            onClick={() => setIsModalOpen(true)}
          />
        </div>
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
