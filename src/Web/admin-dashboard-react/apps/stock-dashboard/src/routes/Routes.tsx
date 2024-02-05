import { RouteObject } from 'react-router-dom';
import { Paths } from './AppNavigation';

import React from 'react';
import { ProtectedRoute } from '../app/ProtectedRoute';

const HomeRoute = React.lazy(() => import('../pods/static/HomeRoute'));
const StockOverviewRoute = React.lazy(() => import('../pods/stock/StockRoute'));

export const routes: RouteObject[] = [
  {
    path: Paths.HOME,
    element: <HomeRoute />,
  },
  {
    path: Paths.STOCK,
    element: (
      <ProtectedRoute>
        <StockOverviewRoute />
      </ProtectedRoute>
    ),
  },
];
