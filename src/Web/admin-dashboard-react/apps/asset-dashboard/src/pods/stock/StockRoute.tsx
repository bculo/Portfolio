import { Outlet } from 'react-router-dom';
import { PageWrapper } from '../../components/PageWrapper';

const StockRoute = () => {
  return (
    <PageWrapper>
      <Outlet></Outlet>
    </PageWrapper>
  );
};

export default StockRoute;
