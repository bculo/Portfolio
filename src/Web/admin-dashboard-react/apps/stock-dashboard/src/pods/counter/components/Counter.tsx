import { increment, decrement } from '../../../stores/counter/counter-slice';
import { useAppDispatch, useAppSelector } from '../../../stores/hooks';

export const Coutner = () => {
  const count = useAppSelector((state) => state.counter.value);
  const dispatch = useAppDispatch();

  return (
    <div>
      <p> {count} </p>
      <button onClick={() => dispatch(increment())}>INCREMENT</button>
      <button onClick={() => dispatch(decrement())}>DECREMENT</button>
    </div>
  );
};
