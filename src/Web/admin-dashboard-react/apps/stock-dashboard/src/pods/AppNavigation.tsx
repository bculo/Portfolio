import { Link } from 'react-router-dom';
import { staticPaths, stockPaths } from '../routes/paths';

type NavigationItem = {
  name: string;
  path: string;
  text: string;
};

const navigation: NavigationItem[] = [];

navigation.push({
  name: 'home',
  path: staticPaths.HOME,
  text: 'Home',
});

navigation.push({
  name: 'stock',
  path: stockPaths.STOCK,
  text: 'Stocks',
});

export const AppNavigation = () => {
  return (
    <ul>
      {navigation.map((item) => {
        return (
          <li key={item.name}>
            <Link to={item.path}>{item.text}</Link>
          </li>
        );
      })}
    </ul>
  );
};
