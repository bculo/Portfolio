type TableProps<T extends object> = {
  columns: Record<
    string,
    {
      name: string;
      accessor: (data: T) => React.ReactNode | string | undefined;
      sortable: boolean;
    }
  >;
  payload: T[];
  totalRecords: number;
  take: number;
  page: number;
  tableSort: TableSortInfo | null;
  onPageChange: (page: number) => void;
  onSortChange: (sort: TableSortInfo) => void | null;
};

export type TableSortInfo = {
  propertyName: string;
  sort: TableSortType;
};

export const Table = <T extends object>({
  columns,
  payload,
  totalRecords,
  take,
  page,
  tableSort,
  onPageChange,
  onSortChange,
}: TableProps<T>) => {
  const isNextVisible = page < Math.ceil(totalRecords / take);
  const isPrevVisible = page > 1;

  return (
    <div>
      <table className="min-w-full divide-y divide-gray-500">
        <thead className="bg-gray-600 text-cyan-500">
          <tr className="text-start">
            {Object.keys(columns).map((key) => {
              const { name, sortable } = columns[key];
              return (
                <th scope="col" key={key} className="px-6 py-3 text-start">
                  <span>{name}</span>{' '}
                  {sortable ? (
                    <TableSortButton
                      sort={
                        tableSort && tableSort.propertyName === name
                          ? tableSort.sort
                          : 'none'
                      }
                      onSortChange={(dir) =>
                        onSortChange &&
                        onSortChange({ propertyName: name, sort: dir })
                      }
                    />
                  ) : null}
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
                      {accessor(rowData)}
                    </td>
                  );
                })}
              </tr>
            );
          })}
        </tbody>
      </table>
      <div className="flex justify-between mt-2">
        <TablePaginationButton
          isVisible={isPrevVisible}
          label="PREV"
          onClick={() => onPageChange(page - 1)}
        />
        <span>Page {page}</span>
        <TablePaginationButton
          isVisible={isNextVisible}
          label="NEXT"
          onClick={() => onPageChange(page + 1)}
        />
      </div>
    </div>
  );
};

type TablePaginationButtonProps = {
  label: string;
  isVisible: boolean;
  onClick: () => void;
};

const TablePaginationButton = ({
  label,
  isVisible,
  onClick,
}: TablePaginationButtonProps) => {
  if (isVisible)
    return (
      <span className="cursor-pointer" onClick={onClick}>
        {label}
      </span>
    );
  return <span>&nbsp;</span>;
};

type TableSortType = 'none' | 'asc' | 'desc';

type TableSortButtonProps = {
  sort: TableSortType;
  onSortChange: (sortDirection: TableSortType) => void;
};

const TableSortButton = ({ sort, onSortChange }: TableSortButtonProps) => {
  return (
    <div>
      {sort === 'asc' && <span onClick={() => onSortChange('desc')}>ASC</span>}
      {sort === 'none' && <span onClick={() => onSortChange('asc')}>-</span>}
      {sort === 'desc' && (
        <span onClick={() => onSortChange('none')}>DESC</span>
      )}
    </div>
  );
};
