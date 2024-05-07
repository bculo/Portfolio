import { yupResolver } from '@hookform/resolvers/yup';
import { FormProvider, SubmitHandler, useForm } from 'react-hook-form';
import * as yup from 'yup';
import { Button, FormField, FormInput } from '@admin-dashboard-react/ui';
import { useAddNewWithDelayMutation } from '../../../stores/crypto/cryptoApiGenerated';

const formSchema = yup.object({
  symbol: yup.string().required('Symbol is required field.'),
});

type Form = yup.InferType<typeof formSchema>;

type Props = {
  onClose: () => void;
};

export const CreateCryptoItemForm = ({ onClose }: Props) => {
  const form = useForm<Form>({
    resolver: yupResolver(formSchema),
  });

  const { handleSubmit } = form;

  const [addWithDelay] = useAddNewWithDelayMutation();

  const onSubmit: SubmitHandler<Form> = (data) => {
    try {
      addWithDelay({ symbol: data.symbol });
      onClose();
    } catch (error) {
      console.error(error);
    }
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
