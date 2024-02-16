import { RouteObject } from 'react-router-dom';
import { stockPaths } from './stockPaths';
import { ProtectedRoute } from '../../app/ProtectedRoute';
import React from 'react';

const StockOverviewPage = React.lazy(
  () => import('../../pods/stock/StockOverviewPage')
);

export const stockRoutes: RouteObject[] = [
  {
    path: stockPaths.OVERVIEW,
    element: (
      <ProtectedRoute>
        <StockOverviewPage />
      </ProtectedRoute>
    ),
  },
];
