import { useFetchPageQuery } from '../../../stores/api/cryptoApiGenerated';
import { CryptoCard } from '../components/CryptoCard';

const CryptoList = () => {
  const { data, isLoading } = useFetchPageQuery({ page: 1, take: 20 });

  return (
    <div className="grid gap-4 grid-cols-2">
      {data &&
        data.map((x) => {
          return (
            <CryptoCard
              id={x.id}
              name={x.name}
              price={x.price}
              symbol={x.symbol}
              website={x.website}
            />
          );
        })}
    </div>
  );
};

export default CryptoList;
