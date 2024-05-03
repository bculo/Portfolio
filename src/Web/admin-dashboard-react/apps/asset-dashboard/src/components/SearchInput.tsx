import { InputHTMLAttributes, useEffect, useState } from 'react';
import { useDebounce } from 'use-debounce';

interface Props extends InputHTMLAttributes<HTMLInputElement> {
  onInputChange: (search: string) => void;
}

export const SearchInput = ({ onInputChange, placeholder }: Props) => {
  const [searchValue, setSearchValue] = useState('');
  const [value] = useDebounce(searchValue, 500);

  useEffect(() => {
    onInputChange(value);
  }, [value, onInputChange]);

  return (
    <input
      placeholder={placeholder}
      className="px-4 py-2 rounded-lg w-48 hover:w-72 focus:w-72 duration-200 bg-gray-900 opacity-70 border-gray-700 border placeholder:italic placeholder:text-stone-600 inline-block read-only:outline-0"
      type="text"
      onChange={(e) => {
        setSearchValue(e.target.value);
      }}
    />
  );
};
