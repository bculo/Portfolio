import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { FormInput } from '../../components/controls/FormInput';
import { Button } from '../../components/Button';
import { FormField } from '../../components/controls/FormField';

export type FilterStockForm = {
  symbol: string;
  status: 1 | 2 | 999;
  page?: number;
  take?: number;
};

type Props = {
  defaultValue: FilterStockForm;
  onSearch: (f: FilterStockForm) => void;
};

export const StockOverviewFilter = ({ defaultValue, onSearch }: Props) => {
  const form = useForm<FilterStockForm>({
    defaultValues: defaultValue,
  });

  const { handleSubmit } = form;

  const onSubmit: SubmitHandler<FilterStockForm> = (data) => onSearch(data);

  return (
    <FormProvider {...form}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormField name="symbol" label="Symbol">
          <FormInput name="symbol" type="text" placeholder="symbol" />
        </FormField>
        <div className="text-right">
          <Button type="submit" text="SUBMIT"></Button>
        </div>
      </form>
    </FormProvider>
  );
};
