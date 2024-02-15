import {
  Controller,
  FormProvider,
  SubmitHandler,
  useForm,
} from 'react-hook-form';
import { FormInput } from '../../components/controls/FormInput';
import { Button } from '../../components/Button';
import { FormSelect } from '../../components/controls/FormSelect';
import {
  ComboboxOption,
  FormCombobox,
} from '../../components/controls/FormCombobox';

type Inputs = {
  example: string;
  option: string;
  more: string;
};

const options: ComboboxOption[] = [
  { value: '1', display: 'Wade Cooper' },
  { value: '2', display: 'Arlene Mccoy' },
  { value: '3', display: 'Devon Webb' },
  { value: '4', display: 'Tom Cook' },
  { value: '5', display: 'Tanya Fox' },
  { value: '6', display: 'Hellen Schmidt' },
];

const StockRoute = () => {
  const form = useForm<Inputs>({
    defaultValues: { example: '123', option: '2', more: '2' },
  });
  const { handleSubmit, control } = form;

  const onSubmit: SubmitHandler<Inputs> = (data) => console.log(data);

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
            <Controller
              name="more"
              control={control}
              render={({ field }) => (
                <FormCombobox
                  onChange={(option) => {
                    field.onChange(option.value);
                  }}
                  options={options}
                  placeholder="More"
                  defaultValue={options[0]}
                />
              )}
            />
            <Button type="submit" text="SUBMIT"></Button>
          </form>
        </FormProvider>
      </div>
      <div></div>
      <p>Total count: {/*data?.totalCount*/}</p>
      {/*data?.items?.map((value, index) => (
        <div key={value.id}>{value.symbol}</div>
      ))*/}
    </div>
  );
};

export default StockRoute;
