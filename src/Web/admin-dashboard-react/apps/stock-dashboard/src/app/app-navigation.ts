import { ComponentType } from 'react';
import { HomeRoute } from '../pods/static/HomeRoute';
import { CounterRoute } from '../pods/counter/CounterRoute';
import { StockOverviewRoute } from '../pods/stock/StockOverviewRoute';

type NavigationItem = {
  name: string;
  path: string;
  text: string;
  component: ComponentType<unknown>;
};

export const navigation: NavigationItem[] = [];

navigation.push({
  name: 'home',
  path: '/',
  text: 'Home',
  component: HomeRoute,
});

navigation.push({
  name: 'counter',
  path: '/counter',
  text: 'Counter',
  component: CounterRoute,
});

navigation.push({
  name: 'stock',
  path: '/stock',
  text: 'Stocks',
  component: StockOverviewRoute,
});

export const AppNavigation = navigation;
