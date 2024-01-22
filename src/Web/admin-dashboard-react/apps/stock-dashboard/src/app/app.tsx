// eslint-disable-next-line @typescript-eslint/no-unused-vars
import CounterRoute from '../pods/counter/CounterRoute';
import { StockOverviewRoute } from '../pods/stock/StockOverviewRoute';

export function App() {
  return (
    <div className="min-h-10 bg-red-100">
      <StockOverviewRoute />
    </div>
  );
}

export default App;
