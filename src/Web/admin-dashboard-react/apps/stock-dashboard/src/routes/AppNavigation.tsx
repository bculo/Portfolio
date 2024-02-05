import { Link } from 'react-router-dom';
import { pathConstants } from './PathConstants';

type NavigationItem = {
  name: string;
  path: string;
  text: string;
};

const navigation: NavigationItem[] = [];

navigation.push({
  name: 'home',
  path: pathConstants.HOME,
  text: 'Home',
});

navigation.push({
  name: 'stock',
  path: pathConstants.STOCK,
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

export const Paths = pathConstants;
