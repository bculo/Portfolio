import { Fragment, useEffect, useState } from 'react';
import { Combobox, Transition } from '@headlessui/react';
import { CheckIcon, ChevronUpDownIcon } from '@heroicons/react/20/solid';

export type ComboboxOption = {
  value: string;
  display: string;
};

type FormComboboxProps = {
  placeholder?: string;
  options: ComboboxOption[];
  onChange: (v: ComboboxOption) => void;
  defaultValue: ComboboxOption | null;
  disabled?: boolean;
};

export const FormCombobox = ({
  disabled,
  placeholder,
  options,
  defaultValue,
  onChange,
}: FormComboboxProps) => {
  const [selected, setSelected] = useState<ComboboxOption | null>(null);
  const [query, setQuery] = useState('');

  const filteredOptions =
    query === ''
      ? options
      : options.filter((person) =>
          person.display
            .toLowerCase()
            .replace(/\s+/g, '')
            .includes(query.toLowerCase().replace(/\s+/g, ''))
        );

  useEffect(() => {
    setSelected(defaultValue);
  }, [defaultValue]);

  return (
    <Combobox
      value={selected}
      onChange={(item) => {
        setSelected(item);
        onChange(item!);
      }}
      disabled={disabled}
    >
      <div className="relative">
        <div className="relative w-full cursor-default overflow-hidden rounded-lg text-left shadow-md focus:outline-none focus-visible:ring-2  sm:text-sm">
          <Combobox.Input
            placeholder={placeholder ?? ''}
            className="w-full border-none py-2 pl-3 pr-10 text-sm leading-5 bg-gray-800  border-gray-700 focus:ring-0 placeholder:italic placeholder:text-stone-600"
            displayValue={(item: ComboboxOption | null) => item?.display ?? ''}
            onChange={(event) => setQuery(event.target.value)}
          />
          <Combobox.Button className="absolute inset-y-0 right-0 flex items-center pr-2">
            <ChevronUpDownIcon
              className="h-5 w-5 text-gray-400"
              aria-hidden="true"
            />
          </Combobox.Button>
        </div>
        <Transition
          as={Fragment}
          leave="transition ease-in duration-100"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
          afterLeave={() => setQuery('')}
        >
          <Combobox.Options className="absolute mt-1 max-h-60 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black/5 focus:outline-none sm:text-sm">
            {filteredOptions.length === 0 ? (
              <div className="relative cursor-default select-none px-4 py-2 text-gray-700">
                Nothing found.
              </div>
            ) : (
              filteredOptions.map((person) => (
                <Combobox.Option
                  key={person.value}
                  className={({ active }) =>
                    `relative cursor-default select-none py-2 pl-10 pr-4 ${
                      active ? 'bg-cyan-700 text-white' : 'text-gray-900'
                    }`
                  }
                  value={person}
                >
                  {({ selected, active }) => (
                    <>
                      <span
                        className={`block truncate ${
                          selected ? 'font-medium' : 'font-normal'
                        }`}
                      >
                        {person.display}
                      </span>
                      {selected ? (
                        <span
                          className={`absolute inset-y-0 left-0 flex items-center pl-3 ${
                            active ? 'text-white' : 'text-teal-600'
                          }`}
                        >
                          <CheckIcon className="h-5 w-5" aria-hidden="true" />
                        </span>
                      ) : null}
                    </>
                  )}
                </Combobox.Option>
              ))
            )}
          </Combobox.Options>
        </Transition>
      </div>
    </Combobox>
  );
};
