import { TableSortInfo } from '../../components/Table';
import { FilterStocksApiArg } from '../../stores/api/stockApiGenerated';
import { StockFilter } from './models';

export function mapToFilterStocksRequest(
  filter: StockFilter,
  sort: TableSortInfo | null
): FilterStocksApiArg {
  if (!sort || sort.sort === 'none') {
    return {
      ...filter,
      'Symbol.Value': filter.symbol,
      'ActivityStatus.Value': filter.status,
    };
  }

  return {
    ...filter,
    'Symbol.Value': filter.symbol,
    'ActivityStatus.Value': filter.status,
    'SortBy.PropertyName': sort.propertyName,
    'SortBy.Direction': sort.sort === 'asc' ? 0 : 1,
  };
}
