import { Suspense } from 'react';
import { useAuth } from 'react-oidc-context';
import { Outlet } from 'react-router-dom';
import { BackgroundPattern } from '../components/BackgroundPattern';
import { SideNavigation } from './SideNavigation';

export const Layout = () => {
  const auth = useAuth();

  return (
    <div className="relative min-h-screen h-full text-stone-300 bg-gradient-to-br from-gray-800 to-gray-900 font-mono text-sm">
      <div className="absolute z-0 w-full flex">
        {auth.isAuthenticated && <SideNavigation />}
        <main className="p-8 flex-1">
          <Suspense fallback={<div>Loading...</div>}>
            <BackgroundPattern />
            <Outlet></Outlet>
          </Suspense>
        </main>
      </div>
    </div>
  );
};
