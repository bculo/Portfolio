import { RouteObject } from 'react-router-dom';
import { stockPaths } from './stockPaths';
import { ProtectedRoute } from '../../app/ProtectedRoute';
import React from 'react';

const StockRoute = React.lazy(() => import('../../pods/stock/StockRoute'));
const StockList = React.lazy(() => import('../../pods/stock/StockList'));

export const stockRoutes: RouteObject[] = [
  {
    path: stockPaths.OVERVIEW,
    element: (
      <ProtectedRoute>
        <StockRoute />
      </ProtectedRoute>
    ),
    children: [
      {
        path: '',
        element: <StockList />,
      },
    ],
  },
];
