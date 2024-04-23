import { RouteObject } from 'react-router-dom';

import { staticRoutes } from './static/StaticRoutes';
import { stockRoutes } from './stock/StockRoutes';

export const routes: RouteObject[] = [
  ...staticRoutes,
  ...stockRoutes,
  { path: '*', element: <div>NOT FOUND</div> },
];
