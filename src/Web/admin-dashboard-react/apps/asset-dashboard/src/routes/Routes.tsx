import { RouteObject } from 'react-router-dom';

import { staticRoutes } from './static/StaticRoutes';
import { stockRoutes } from './stock/StockRoutes';
import { cryptoRoutes } from './crypto/CryptoRoutes';

export const routes: RouteObject[] = [
  ...staticRoutes,
  ...stockRoutes,
  ...cryptoRoutes,
  { path: '*', element: <div>NOT FOUND</div> },
];
