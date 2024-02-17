import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { FormField } from '../../components/controls/FormField';
import { FormInput } from '../../components/controls/FormInput';
import { Button } from '../../components/Button';
import { useCreateStockMutation } from '../../stores/api/generated';

type Form = {
  symbol: string;
};

type Props = {
  onCreated: () => void;
};

export const CreateStockForm = ({ onCreated }: Props) => {
  const form = useForm<Form>();

  const [createStock, { isLoading }] = useCreateStockMutation();

  const { handleSubmit } = form;

  const onSubmit: SubmitHandler<Form> = async (data) => {
    await createStock({ symbol: data.symbol });
    onCreated();
  };

  return (
    <div className="text-stone-300">
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
    </div>
  );
};
