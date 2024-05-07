import { Button } from '@admin-dashboard-react/ui';
import clsx from 'clsx';
import { useUndoAddNewDelayMutation } from '../../../stores/crypto/cryptoApiGenerated';
import { useAppSelector } from '../../../stores/hooks';

type Props = {
  visible: boolean;
};

export const CancelCreateCrytoItemToast = ({ visible }: Props) => {
  const temporaryId = useAppSelector((state) => state.cryptoSlice.temporaryId);
  const [undoAction] = useUndoAddNewDelayMutation();

  return (
    <div
      className={clsx(
        'fixed bottom-10 right-10 glass z-20 flex gap-8 p-12 rounded-lg',
        {
          hidden: !visible,
        }
      )}
    >
      <span>Cancel create action?</span>
      <Button
        text="Cancel"
        buttonStyle="full"
        type="button"
        onClick={() => undoAction({ temporaryId: temporaryId! })}
      />
    </div>
  );
};
