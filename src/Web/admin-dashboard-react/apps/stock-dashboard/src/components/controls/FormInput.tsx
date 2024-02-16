import { InputHTMLAttributes } from 'react';
import { useFormContext } from 'react-hook-form';

interface FormInputProps extends InputHTMLAttributes<HTMLInputElement> {
  name: string;
  label?: string;
}

export const FormInput = ({
  type,
  name = '',
  placeholder,
}: Partial<FormInputProps>) => {
  const { register } = useFormContext();

  return (
    <input
      placeholder={placeholder}
      className="px-4 py-2 rounded-lg bg-gray-900 opacity-70 border-gray-700 w-full border placeholder:italic placeholder:text-stone-600 inline-block read-only:outline-0"
      type={type}
      {...register(name)}
    />
  );
};
