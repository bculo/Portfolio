import { useCallback, useEffect, useState } from 'react';
import { format } from 'date-fns';
import {
  useChangeStockStatusMutation,
  useFilterStocksQuery,
} from '../../../stores/stock/stockApiGenerated';
import { Table, TableSortInfo } from '@admin-dashboard-react/ui';
import { Button } from '@admin-dashboard-react/ui';
import { Modal } from '@admin-dashboard-react/ui';
import { CreateStockForm } from '../components/CreateStockForm';
import { SearchInput } from '@admin-dashboard-react/ui';
import {
  ArrowTopRightOnSquareIcon,
  XCircleIcon,
} from '@heroicons/react/20/solid';
import { Spinner } from '@admin-dashboard-react/ui';
import { useNavigate } from 'react-router-dom';
import { WebSocketService } from '../../../services/interfaces';
import { myContainer } from '../../../services/inversify.config';
import { TYPES } from '../../../services/types';
import { StockFilter } from '../models';
import { mapToFilterStocksRequest } from '../mappers';

const defaultVal: StockFilter = {
  status: 999,
  symbol: '',
  page: 1,
  take: 20,
};

const webSocketService = myContainer.get<WebSocketService>(
  TYPES.WebSocketService
);

const StockList = () => {
  const navigate = useNavigate();
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [filter, setFilter] = useState<StockFilter>(defaultVal);
  const [sort, setSort] = useState<TableSortInfo | null>(null);
  const {
    data,
    refetch,
    isLoading: filterIsLoading,
  } = useFilterStocksQuery(mapToFilterStocksRequest(filter, sort), {
    pollingInterval: 60000,
  });
  const [changeStatus, { isLoading: statusIsLoading }] =
    useChangeStockStatusMutation();

  const isLoading = filterIsLoading || statusIsLoading;

  useEffect(() => {
    webSocketService.joinGroup('Stock.StatusChanged', 'stock', () => refetch());
    return () => webSocketService.leaveGroup('Stock.StatusChanged', 'stock');
  }, [refetch]);

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search }));
  }, []);

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
          tableSort={sort}
          take={filter.take}
          page={filter.page}
          onSortChange={(sort) => setSort(sort)}
          onPageChange={(page) =>
            setFilter((prev) => ({ ...prev, page: page }))
          }
          totalRecords={data?.totalCount ?? 0}
          payload={data?.items ?? []}
          columns={{
            symbol: {
              sortable: true,
              name: 'Symbol',
              accessor: (item) => item.symbol,
            },
            lastPriceUpdate: {
              sortable: false,
              name: 'Last update',
              accessor: (item) => {
                return item.lastPriceUpdate
                  ? format(new Date(item.lastPriceUpdate), 'dd.MM.yyyy')
                  : '';
              },
            },
            price: {
              sortable: true,
              name: 'Price',
              accessor: (item) => (
                <span className="underline underline-offset-4">
                  {item.price}
                </span>
              ),
            },
            active: {
              sortable: false,
              name: 'Active',
              accessor: (item) =>
                item.isActive ? (
                  <span className="text-green-600">Active</span>
                ) : (
                  <span className="text-red-600">Unactive</span>
                ),
            },
            actions: {
              sortable: false,
              name: 'Actions',
              accessor: (item) => (
                <div className="flex gap-x-2">
                  <XCircleIcon
                    className="h-6 w-6 text-red-600 hover:cursor-pointer"
                    onClick={() => changeStatus({ id: item.id })}
                  />
                  <ArrowTopRightOnSquareIcon
                    className="h-6 w-6 text-cyan-700 hover:cursor-pointer"
                    onClick={() => navigate(`${item.id}`)}
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
