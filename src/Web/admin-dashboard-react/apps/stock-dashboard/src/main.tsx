import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom/client';
import { store } from './stores/store';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

import App from './app/app';
import { ErrorRoute } from './pods/static/ErrorRoute';
import { HomeRoute } from './pods/static/HomeRoute';
import { CounterRoute } from './pods/counter/CounterRoute';
import { StockOverviewRoute } from './pods/stock/StockOverviewRoute';
import { AuthProvider } from 'react-oidc-context';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        path: '/',
        element: <HomeRoute />,
      },
      {
        path: '/counter',
        element: <CounterRoute />,
      },
      {
        path: '/stock',
        element: <StockOverviewRoute />,
      },
    ],
    errorElement: <ErrorRoute />,
  },
]);

const oidcConfig = {
  authority: 'http://localhost:8080/realms/PortfolioRealm/',
  client_id: 'Stock.Client',
  redirect_uri: 'http://localhost:4200',
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <StrictMode>
    <AuthProvider {...oidcConfig}>
      <Provider store={store}>
        <RouterProvider router={router} />
      </Provider>
    </AuthProvider>
  </StrictMode>
);
