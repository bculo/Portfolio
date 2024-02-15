import { RouterProvider, createBrowserRouter } from 'react-router-dom';

import { Layout } from './Layout';
import { routes } from '../pods/Routes';

const router = createBrowserRouter([
  {
    element: <Layout />,
    errorElement: <div>ERROR</div>,
    children: routes,
  },
]);

export function App() {
  return <RouterProvider router={router} />;
}

export default App;
