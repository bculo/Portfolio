import { NavLink } from 'react-router-dom';
import { staticPaths, stockPaths } from '../routes/paths';
import { ChartBarIcon, HomeIcon } from '@heroicons/react/20/solid';
import { cryptoPaths } from '../routes/crypto/cryptoPaths';

type NavigationItem = {
  name: string;
  path: string;
  text: string;
  icon: React.ReactNode;
};

const navigation: NavigationItem[] = [];

navigation.push({
  name: 'home',
  path: staticPaths.HOME,
  text: 'Home',
  icon: <HomeIcon />,
});

navigation.push({
  name: 'stock',
  path: stockPaths.OVERVIEW,
  text: 'Stocks',
  icon: <ChartBarIcon />,
});

navigation.push({
  name: 'crypto',
  path: cryptoPaths.OVERVIEW,
  text: 'Cryptos',
  icon: <ChartBarIcon />,
});

export const NavigationList = () => {
  return (
    <ul>
      {navigation.map((item) => {
        return (
          <li key={item.name}>
            <NavLink
              to={item.path}
              className={({ isActive }) =>
                [
                  isActive ? 'bg-gray-800 text-cyan-500' : '',
                  'w-full flex items-center gap-x-4 mb-1 px-4 py-2 hover:text-cyan-500 hover:bg-gray-800 duration-150',
                ].join(' ')
              }
            >
              <span className="inline-block h-6 w-6">{item.icon}</span>
              {item.text}
            </NavLink>
          </li>
        );
      })}
    </ul>
  );
};
