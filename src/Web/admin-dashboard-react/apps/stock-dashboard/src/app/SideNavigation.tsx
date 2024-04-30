import { useAuth } from 'react-oidc-context';
import { NavigationList } from './NavigationList';

export const SideNavigation = () => {
  const auth = useAuth();

  return (
    <aside className="flex-none flex flex-col justify-between w-[20rem] glass py-8 h-screen rounded-tr-2xl rounded-br-2xl">
      <div>
        <h2 className="text-stone-200 text-center font-bold tracking-wider text-2xl">
          Stock dashboard
        </h2>
        <nav className="mt-10">
          <NavigationList />
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
  );
};
