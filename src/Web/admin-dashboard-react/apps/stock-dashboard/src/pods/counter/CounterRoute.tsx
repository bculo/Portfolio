import { useAppSelector } from '../../stores/hooks';
import { Coutner } from './components/Counter';

export const CounterRoute = () => {
  const count = useAppSelector((state) => state.counter.value);

  return (
    <div>
      <div>{count}</div>

      <Coutner />
      <Coutner />
    </div>
  );
};
