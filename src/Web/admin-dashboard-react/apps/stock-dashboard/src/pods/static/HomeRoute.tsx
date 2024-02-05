import { useAuth } from 'react-oidc-context';
import { Button } from '../../components/Button';

const HomeRoute = () => {
  const auth = useAuth();

  if (auth.isAuthenticated) {
    return <div>HOME ROUTE</div>;
  }

  return (
    <div>
      <Button
        buttonStyle="full"
        text="LOGIN"
        type="button"
        onClick={() => auth.signinRedirect()}
      />
    </div>
  );
};

export default HomeRoute;
