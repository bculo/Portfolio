import React from 'react';
import clsx from 'clsx';
import { useFormContext } from 'react-hook-form';

type Props = {
  name: string;
  label: string;
  children: React.ReactNode;
};

export const FormField = ({ name, label, children }: Props) => {
  const {
    formState: { errors },
  } = useFormContext();

  return (
    <div className="mb-2">
      <p className="font-bold text-base pb-1">{label}:</p>
      <div>{children}</div>
      <p className={clsx('text-red-500')}>
        {(errors[name]?.message as string) ?? ''}
      </p>
    </div>
  );
};
