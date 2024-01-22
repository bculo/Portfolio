import { useAppSelector } from '../../stores/hooks';
import { Coutner } from './components/Counter';

/* eslint-disable-next-line */
export interface CounterRouteProps {}

export function CounterRoute(props: CounterRouteProps) {
  const count = useAppSelector((state) => state.counter.value);

  return (
    <div>
      <div>{count}</div>

      <Coutner />
      <Coutner />
    </div>
  );
}

export default CounterRoute;
