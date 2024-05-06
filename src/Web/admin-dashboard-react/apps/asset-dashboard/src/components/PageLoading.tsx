import { Spinner } from '@admin-dashboard-react/ui';

export const PageLoading = () => {
  return (
    <div className="relative h-full w-full">
      <Spinner visible={true} />
    </div>
  );
};
