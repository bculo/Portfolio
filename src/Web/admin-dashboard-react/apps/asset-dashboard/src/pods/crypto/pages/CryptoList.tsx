import { useCallback, useState } from 'react';
import {
  FetchPageApiArg,
  useFetchPageQuery,
} from '../../../stores/api/cryptoApiGenerated';
import { CryptoCard } from '../components/CryptoCard';
import { SearchInput } from '@admin-dashboard-react/ui';

const defaultQuery: FetchPageApiArg = {
  page: 1,
  take: 20,
  symbol: '',
};

const CryptoList = () => {
  const [filter, setFilter] = useState<FetchPageApiArg>(defaultQuery);
  const { data } = useFetchPageQuery(filter);

  const onSearchChange = useCallback((search: string) => {
    setFilter((prev) => ({ ...prev, symbol: search }));
  }, []);

  return (
    <div>
      <div className="flex justify-center mb-4">
        <SearchInput
          placeholder="search by symbol"
          onInputChange={onSearchChange}
        />
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
    </div>
  );
};

export default CryptoList;
