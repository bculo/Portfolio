import { RouteObject } from 'react-router-dom';

import { statisRoutes } from './static/StaticRoutes';
import { stockRoutes } from './stock/StockRoutes';

export const routes: RouteObject[] = [...statisRoutes, ...stockRoutes];
