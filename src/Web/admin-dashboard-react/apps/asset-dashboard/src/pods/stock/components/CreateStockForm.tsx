import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import { FormField } from '@admin-dashboard-react/ui';
import { FormInput } from '@admin-dashboard-react/ui';
import { Button } from '@admin-dashboard-react/ui';
import { useCreateStockMutation } from '../../../stores/stock';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

const formSchema = yup.object({
  symbol: yup.string().required('Symbol is required field.'),
});

type Form = yup.InferType<typeof formSchema>;

type Props = {
  onCreated: () => void;
};

export const CreateStockForm = ({ onCreated }: Props) => {
  const form = useForm<Form>({
    resolver: yupResolver(formSchema),
  });

  const [createStock] = useCreateStockMutation();

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
