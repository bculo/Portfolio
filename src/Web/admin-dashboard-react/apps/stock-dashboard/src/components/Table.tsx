type TableProps<T extends object> = {
  columns: Record<
    string,
    {
      name: string;
      accessor: (data: T) => React.ReactNode | string | undefined;
    }
  >;
  payload: T[];
};

export const Table = <T extends object>({
  columns,
  payload,
}: TableProps<T>) => {
  return (
    <table className="min-w-full divide-y divide-gray-500">
      <thead className="bg-gray-600">
        <tr className="text-start">
          {Object.keys(columns).map((key) => {
            const { name } = columns[key];
            return (
              <th scope="col" key={key} className="px-6 py-3 text-start">
                {name}
              </th>
            );
          })}
        </tr>
      </thead>

      <tbody className="divide-y divide-gray-500">
        {payload.map((rowData, index) => {
          return (
            <tr key={index} className="px-6 py-3">
              {Object.keys(columns).map((key) => {
                const { accessor } = columns[key];
                return (
                  <td className="px-6 py-3 text-start" key={key}>
                    {accessor ? accessor(rowData) : 'HELLO'}
                  </td>
                );
              })}
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};
