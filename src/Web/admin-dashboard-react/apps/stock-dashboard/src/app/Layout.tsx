import { Suspense } from 'react';
import { useAuth } from 'react-oidc-context';
import { Outlet } from 'react-router-dom';
import { AppNavigation } from '../pods/AppNavigation';

export const Layout = () => {
  const auth = useAuth();

  return (
    <div className="relative min-h-screen h-full text-stone-300 bg-gradient-to-br from-gray-800 to-gray-900 font-mono text-sm">
      <div className="absolute z-0 w-full flex">
        {auth.isAuthenticated && (
          <aside className="flex-none flex flex-col justify-between w-[20rem] glass py-8 h-screen rounded-tr-2xl rounded-br-2xl">
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
        <main className="p-8 flex-1">
          <Suspense fallback={<div>Loading...</div>}>
            <div className="w-full h-full absolute top-0 left-0 -z-50">
              <svg
                id="patternId"
                width="100%"
                height="100%"
                xmlns="http://www.w3.org/2000/svg"
              >
                <defs>
                  <pattern
                    id="a"
                    patternUnits="userSpaceOnUse"
                    width="25.5"
                    height="26"
                    patternTransform="scale(5) rotate(20)"
                  >
                    <rect
                      x="0"
                      y="0"
                      width="100%"
                      height="100%"
                      fill="hsla(0, 0%, 100%, 0)"
                    />
                    <path
                      d="M10-6V6M10 14v12M26 10H14M6 10H-6"
                      transform="translate(2.75,0)"
                      stroke="hsla(259, 46%, 51%, 0.06)"
                      fill="none"
                    />
                  </pattern>
                </defs>
                <rect
                  width="800%"
                  height="800%"
                  transform="translate(-175,0)"
                  fill="url(#a)"
                />
              </svg>
            </div>
            <Outlet></Outlet>
          </Suspense>
        </main>
      </div>
      <div className="absolute -z-50 bg-gray-700 w-full h-full"></div>
    </div>
  );
};
