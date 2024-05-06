import React from 'react';
import { RouteObject } from 'react-router-dom';
import { cryptoPaths } from './cryptoPaths';

const HomeRoute = React.lazy(() => import('../../pods/crypto/CryptoRoute'));

export const cryptoRoutes: RouteObject[] = [
  {
    path: cryptoPaths.OVERVIEW,
    element: <HomeRoute />,
  },
];
