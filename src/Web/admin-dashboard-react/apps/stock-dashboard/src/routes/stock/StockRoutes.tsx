import { RouteObject } from 'react-router-dom';
import { stockPaths } from './stockPaths';
import { ProtectedRoute } from '../../app/ProtectedRoute';
import React from 'react';

const StockOverviewRoute = React.lazy(
  () => import('../../pods/stock/StockRoute')
);

export const stockRoutes: RouteObject[] = [
  {
    path: stockPaths.STOCK,
    element: (
      <ProtectedRoute>
        <StockOverviewRoute />
      </ProtectedRoute>
    ),
  },
];
