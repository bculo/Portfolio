import React, { InputHTMLAttributes } from 'react';
import { useFormContext } from 'react-hook-form';

export interface Option {
  value: string;
  label: string;
}

interface FormSelectProps extends InputHTMLAttributes<HTMLInputElement> {
  name: string;
  label?: string;
  options: Option[];
}

export const FormSelect = ({ name, options, label }: FormSelectProps) => {
  const {
    register,
    formState: { errors },
  } = useFormContext();

  return (
    <div className="relative">
      <select
        {...register(name)}
        className="px-4 py-[.6rem] rounded-lg bg-gray-900 opacity-70 w-full border-gray-700 border placeholder:italic placeholder:text-stone-600 inline-block"
      >
        {options.map((item) => (
          <option key={item.value} value={item.value}>
            {item.label}
          </option>
        ))}
      </select>
    </div>
  );
};
