import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { FormInput } from '../../components/controls/FormInput';
import { useGetStocksQuery } from '../../stores/api/generated';
import { Button } from '../../components/Button';
import { FormSelect } from '../../components/controls/FormSelect';

type Inputs = {
  example: string;
  option: string;
};

const StockRoute = () => {
  const form = useForm<Inputs>({
    defaultValues: { example: '123', option: '2' },
  });
  const { handleSubmit } = form;

  const onSubmit: SubmitHandler<Inputs> = (data) => console.log(data);

  const { data, isFetching } = useGetStocksQuery();
  return (
    <div>
      <div>
        <FormProvider {...form}>
          <form onSubmit={handleSubmit(onSubmit)}>
            <FormInput name="example" type="text" />
            <FormSelect
              name="option"
              options={[
                { label: '1', value: '1' },
                { label: '2', value: '2' },
              ]}
            ></FormSelect>
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
