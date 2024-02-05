import React, { Suspense } from 'react';
import { useAuth } from 'react-oidc-context';
import { Outlet } from 'react-router-dom';
import { AppNavigation } from '../routes/AppNavigation';

export const Layout = () => {
  const auth = useAuth();

  return (
    <div className="relative min-h-screen h-full text-stone-300 font-mono text-sm">
      <div className="absolute z-0 w-full flex">
        {auth.isAuthenticated && (
          <aside className="flex-none flex flex-col justify-between w-[20rem] max-2xl:w-[17rem] glass py-8 h-screen rounded-tr-2xl rounded-br-2xl">
            <div>
              <h2 className="text-stone-200 text-center font-bold tracking-wider text-2xl">
                Stock dashboard
              </h2>
              <nav className="mt-10">
                <AppNavigation />
              </nav>
            </div>
            <div className="flex justify-between items-center px-8">
              <div className="flex items-center">
                <p className="pl-4 text-md">{auth.user?.profile.family_name}</p>
              </div>
              <p
                className="hover:text-cyan-500 cursor-pointer"
                onClick={() => auth.signoutPopup()}
              >
                LOGOUT
              </p>
            </div>
          </aside>
        )}
        <main className="p-8">
          <Suspense fallback={<div>Loading...</div>}>
            <Outlet></Outlet>
          </Suspense>
        </main>
      </div>
      <div className="absolute -z-50 bg-gray-700 w-full h-full"></div>
    </div>
  );
};
