import { Spinner } from '../../../../libs/ui/src/lib/Spinner';

export const PageLoading = () => {
  return (
    <div className="relative h-full w-full">
      <Spinner visible={true} />
    </div>
  );
};
