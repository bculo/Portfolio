type ButtonStyleType = 'empty' | 'full';
type ButtonType = 'submit' | 'button';

type Props = {
  buttonStyle: ButtonStyleType;
  type: ButtonType;
  text: string;
  onClick?: () => void;
};

export const Button = ({
  type = 'button',
  buttonStyle = 'full',
  text = '',
  onClick,
}: Partial<Props>) => {
  const isEmptyStyle = buttonStyle === 'empty';

  if (isEmptyStyle) {
    return (
      <button
        type={type}
        onClick={onClick}
        className="box-border text-cyan-700 px-2 py-2 border-cyan-700 border rounded-md min-w-32 transition-all delay-75 ease-out"
      >
        {text}
      </button>
    );
  }

  return (
    <button
      type={type}
      onClick={onClick}
      className="box-border bg-cyan-700 px-4 py-2 hover:bg-cyan-800 rounded-md min-w-32 transition-all delay-75 ease-out"
    >
      {text}
    </button>
  );
};
