import { PropsWithChildren } from 'react';
import { useAuth } from 'react-oidc-context';

type Props = PropsWithChildren;

export const ProtectedRoute = ({ children }: Props) => {
  const auth = useAuth();

  if (auth.isLoading && !auth.isAuthenticated) {
    return 'Loading...';
  }

  if (!auth.isLoading && !auth.isAuthenticated) {
    return 'Not authenticated';
  }

  return children;
};
