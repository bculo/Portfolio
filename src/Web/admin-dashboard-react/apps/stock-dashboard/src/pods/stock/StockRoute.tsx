import { useGetStocksQuery } from '../../stores/api/generated';

const StockRoute = () => {
  console.log('STOCK OVERVIEW ROUTE');

  const { data, isFetching } = useGetStocksQuery();
  return (
    <div>
      <p>Total count: {data?.length}</p>
      {data?.map((value, index) => (
        <div key={value.id}>{value.symbol}</div>
      ))}
    </div>
  );
};

export default StockRoute;
