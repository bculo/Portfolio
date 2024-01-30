import { useGetAllQuery } from '../../stores/api/generated';

export const StockOverviewRoute = () => {
  const { data, isFetching } = useGetAllQuery();
  return (
    <div>
      <p>Total count: {data?.length}</p>
      {data?.map((value, index) => (
        <div key={value.id}>{value.symbol}</div>
      ))}
    </div>
  );
};