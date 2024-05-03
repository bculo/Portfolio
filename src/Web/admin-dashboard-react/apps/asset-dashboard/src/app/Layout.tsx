import { Suspense, useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { Outlet } from 'react-router-dom';
import { BackgroundPattern } from '../components/BackgroundPattern';
import { SideNavigation } from './SideNavigation';
import { PageLoading } from '../components/PageLoading';
import { myContainer } from '../services/inversify.config';
import { WebSocketService } from '../services/interfaces';
import { TYPES } from '../services/types';
import { Spinner } from '../components/Spinner';

const webSocketService = myContainer.get<WebSocketService>(
  TYPES.WebSocketService
);

export const Layout = () => {
  const auth = useAuth();

  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (auth.isAuthenticated) {
      webSocketService.connect((_) => setIsLoading(false));
      return;
    }
    const timeoutId = setTimeout(() => setIsLoading(false), 2000);
    return () => clearTimeout(timeoutId);
  }, [auth.isAuthenticated]);

  return (
    <div className="relative min-h-screen text-stone-300 bg-gradient-to-br from-gray-800 to-gray-900 font-mono text-sm">
      <div className="absolute z-0 w-full h-full flex">
        {auth.isAuthenticated && <SideNavigation />}
        {isLoading ? (
          <Spinner visible={isLoading} />
        ) : (
          <main className="p-8 flex-1">
            <Suspense fallback={<PageLoading />}>
              <BackgroundPattern />
              <Outlet></Outlet>
            </Suspense>
          </main>
        )}
      </div>
    </div>
  );
};
