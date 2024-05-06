import { Outlet } from 'react-router-dom';
import { PageWrapper } from '../../components/PageWrapper';

const CryptoRoute = () => {
  return (
    <PageWrapper>
      <Outlet></Outlet>
    </PageWrapper>
  );
};

export default CryptoRoute;
