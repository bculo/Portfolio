import { useCallback, useState } from 'react';
import {
  FetchPageApiArg,
  useFetchPageQuery,
} from '../../../stores/api/cryptoApiGenerated';
import { CryptoCard } from '../components/CryptoCard';
import { Button, Modal, SearchInput } from '@admin-dashboard-react/ui';
import { CreateCryptoItemForm } from '../components/CreateCryptoItemForm';

const defaultQuery: FetchPageApiArg = {
  page: 1,
  take: 20,
  symbol: '',
};

const CryptoList = () => {
  const [modalVisible, setModalVisible] = useState<boolean>(false);
  const [filter, setFilter] = useState<FetchPageApiArg>(defaultQuery);
  const { data } = useFetchPageQuery(filter);

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search }));
  }, []);

  return (
    <div>
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
      <div className="grid gap-4 grid-cols-2">
        {data &&
          data.map((x) => {
            return (
              <CryptoCard
                key={x.id}
                id={x.id}
                name={x.name}
                price={x.price}
                symbol={x.symbol}
                website={x.website}
              />
            );
          })}
      </div>
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
