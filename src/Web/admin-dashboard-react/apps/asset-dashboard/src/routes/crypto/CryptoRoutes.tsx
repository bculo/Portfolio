import React from 'react';
import { RouteObject } from 'react-router-dom';
import { cryptoPaths } from './cryptoPaths';
import { ProtectedRoute } from '../../app/ProtectedRoute';

const CryptoRoute = React.lazy(() => import('../../pods/crypto/CryptoRoute'));
const CryptoList = React.lazy(
  () => import('../../pods/crypto/pages/CryptoList')
);

export const cryptoRoutes: RouteObject[] = [
  {
    path: cryptoPaths.OVERVIEW,
    element: (
      <ProtectedRoute>
        <CryptoRoute />
      </ProtectedRoute>
    ),
    children: [
      {
        path: '',
        element: <CryptoList />,
      },
    ],
  },
];
