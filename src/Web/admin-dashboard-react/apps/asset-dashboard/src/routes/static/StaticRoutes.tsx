import React from 'react';
import { RouteObject } from 'react-router-dom';
import { staticPaths } from './staticPaths';

const HomeRoute = React.lazy(() => import('../../pods/static/HomeRoute'));

export const staticRoutes: RouteObject[] = [
  {
    path: staticPaths.HOME,
    element: <HomeRoute />,
  },
];
