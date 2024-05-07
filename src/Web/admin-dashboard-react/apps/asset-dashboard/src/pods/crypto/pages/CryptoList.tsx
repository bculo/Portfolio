import { useCallback, useState } from 'react';
import { FetchPageApiArg, useFetchPageQuery } from '../../../stores/crypto';
import { useAppSelector } from '../../../stores/hooks';
import { Button, Modal, SearchInput, Table } from '@admin-dashboard-react/ui';
import { CreateCryptoItemForm } from '../components/CreateCryptoItemForm';
import { CancelCreateCrytoItemToast } from '../components/CancelCreateCrytoItemToast';
import { ArrowTopRightOnSquareIcon } from '@heroicons/react/20/solid';
import { useNavigate } from 'react-router-dom';

const defaultQuery: FetchPageApiArg = {
  page: 1,
  take: 20,
  symbol: '',
};

const CryptoList = () => {
  const navigate = useNavigate();
  const [modalVisible, setModalVisible] = useState<boolean>(false);
  const [filter, setFilter] = useState<FetchPageApiArg>(defaultQuery);
  const { data } = useFetchPageQuery(filter);
  const cancelToastVisible = useAppSelector(
    (x) => x.cryptoSlice.cancelToastVisible
  );

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search, page: 1 }));
  }, []);

  return (
    <div className="glass p-4 rounded-md">
      <div className="flex justify-between items-center mb-4">
        <div>
          <SearchInput
            placeholder="search by symbol"
            onInputChange={onSearchChange}
          />
        </div>
        <div>
          <Button
            text="Create"
            buttonStyle="full"
            type="button"
            onClick={() => setModalVisible(true)}
          />
        </div>
      </div>
      <div>
        <Table
          tableSort={null}
          take={filter.take!}
          page={filter.page!}
          onSortChange={(_) => {}}
          onPageChange={(page) =>
            setFilter((prev) => ({ ...prev, page: page }))
          }
          totalRecords={data?.totalCount ?? 0}
          payload={data?.items ?? []}
          columns={{
            symbol: {
              sortable: false,
              name: 'Symbol',
              accessor: (item) => item.symbol,
            },
            price: {
              sortable: false,
              name: 'Price',
              accessor: (item) => (
                <span className="underline underline-offset-4">
                  {item.price}
                </span>
              ),
            },
            actions: {
              sortable: false,
              name: 'Actions',
              accessor: (item) => (
                <div className="flex gap-x-2">
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
      <CancelCreateCrytoItemToast visible={cancelToastVisible} />
      <Modal
        isOpen={modalVisible}
        title="Create crypto item"
        onModalClose={() => setModalVisible(false)}
      >
        <CreateCryptoItemForm onClose={() => setModalVisible(false)} />
      </Modal>
    </div>
  );
};

export default CryptoList;
