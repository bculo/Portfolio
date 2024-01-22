import { NavLink } from 'react-router-dom';
import { AppNavigation } from './app-navigation';
import { Outlet } from 'react-router-dom';

export function App() {
  return (
    <div>
      <ul>
        {AppNavigation.map((item) => (
          <li key={item.name}>
            <NavLink to={item.path}>{item.text}</NavLink>
          </li>
        ))}
      </ul>
      <main>
        {' '}
        <Outlet />
      </main>
    </div>
  );
}

export default App;
