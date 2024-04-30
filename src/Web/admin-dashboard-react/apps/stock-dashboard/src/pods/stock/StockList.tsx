import { useCallback, useState } from 'react';
import { format } from 'date-fns';
import {
  FilterStocksApiArg,
  StockPriceResultDto,
  useChangeStockStatusMutation,
  useFilterStocksQuery,
} from '../../stores/api/generated';
import { Table } from '../../components/Table';
import { Button } from '../../components/Button';
import { Modal } from '../../components/Modal';
import { CreateStockForm } from './CreateStockForm';
import { SearchInput } from '../../components/SearchInput';
import {
  ArrowTopRightOnSquareIcon,
  XCircleIcon,
} from '@heroicons/react/20/solid';
import { Spinner } from '../../components/Spinner';
import { useNavigate } from 'react-router-dom';

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

const StockList = () => {
  const navigate = useNavigate();
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [filter, setFilter] = useState<StockFilter>(defaultVal);
  const { data, refetch, isLoading } = useFilterStocksQuery(map(filter));
  const [changeStatus] = useChangeStockStatusMutation();

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search }));
  }, []);

  const deactivateItem = async (item: StockPriceResultDto) => {
    await changeStatus({ id: item.id });
    refetch();
  };

  const routeToAsset = (item: StockPriceResultDto) => {
    navigate(`${item.symbol}`);
  };

  return (
    <div className="glass p-4 rounded-md">
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

      <div className="mt-4 relative">
        <Spinner visible={isLoading} wholeScreen={false} />
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
            price: {
              name: 'Price',
              accessor: (item) => (
                <span className="underline underline-offset-4">
                  {item.price}
                </span>
              ),
            },
            active: {
              name: 'Active',
              accessor: (item) =>
                item.isActive ? (
                  <span className="text-green-600">Active</span>
                ) : (
                  <span className="text-red-600">Unactive</span>
                ),
            },
            actions: {
              name: 'Actions',
              accessor: (item) => (
                <div className="flex gap-x-2">
                  <XCircleIcon
                    className="h-6 w-6 text-red-600 hover:cursor-pointer"
                    onClick={() => deactivateItem(item)}
                  />
                  <ArrowTopRightOnSquareIcon
                    className="h-6 w-6 text-cyan-700 hover:cursor-pointer"
                    onClick={() => routeToAsset(item)}
                  />
                </div>
              ),
            },
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

export default StockList;
