import clsx from 'clsx';

type Props = {
  wholeScreen: boolean;
  visible: boolean;
};

export const Spinner = ({ visible, wholeScreen }: Props) => {
  return (
    <div
      className={clsx(
        'flex justify-center items-center top-0 left-0 z-50 w-full h-full',
        {
          fixed: wholeScreen && visible,
          absolute: !wholeScreen && visible,
          hidden: !visible,
        }
      )}
    >
      <div className="lds-roller">
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </div>
  );
};
