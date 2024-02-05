import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { FormInput } from '../../components/controls/FormInput';
import { useGetStocksQuery } from '../../stores/api/generated';
import { Button } from '../../components/Button';

type Inputs = {
  example: string;
};

const StockRoute = () => {
  const form = useForm<Inputs>();
  const { handleSubmit } = form;

  const onSubmit: SubmitHandler<Inputs> = (data) => console.log(data);

  const { data, isFetching } = useGetStocksQuery();
  return (
    <div>
      <div>
        <FormProvider {...form}>
          <form onSubmit={handleSubmit(onSubmit)}>
            <FormInput name="example" type="text" />
            <Button type="submit" text="SUBMIT"></Button>
          </form>
        </FormProvider>
      </div>
      <p>Total count: {data?.length}</p>
      {data?.map((value, index) => (
        <div key={value.id}>{value.symbol}</div>
      ))}
    </div>
  );
};

export default StockRoute;
