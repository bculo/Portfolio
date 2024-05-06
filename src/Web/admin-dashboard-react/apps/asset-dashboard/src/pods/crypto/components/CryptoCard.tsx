export type Props = {
  id?: string;
  symbol?: string;
  name?: string;
  website?: string;
  price?: number;
};

export const CryptoCard = (props: Props) => {
  return (
    <div className="glass p-8 rounded-lg hover:cursor-pointer">
      <div className="flex justify-between">
        <div>
          <span className="text-cyan-500 font-bold text-lg">
            {props.symbol}
          </span>
          <span className="ml-3">({props.name})</span>
        </div>
        <span className="font-bold underline underline-offset-2">
          ${props.price}
        </span>
      </div>
    </div>
  );
};
