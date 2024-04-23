import { useAuth } from 'react-oidc-context';
import { Button } from '../../components/Button';
import landingPage from '../../assets/landing-page.png';

const HomeRoute = () => {
  const auth = useAuth();

  return (
    <div className="flex p-16 h-full">
      <div className="flex-1 flex flex-col justify-center text-white relative">
        <h1 className="text-5xl text-right tracking-wide leading-snug">
          Strategic Asset Mastery: Elevate Your Portfolio with Seamless{' '}
          <span className="text-cyan-500 font-extrabold underline underline-offset-4">
            Stock
          </span>{' '}
          Management
        </h1>
        <p className="text-right text-lg mt-4 text-stone-300">
          A Quest for the Ultimate Stock Management
        </p>
        <div className="text-right mt-6">
          {!auth.isAuthenticated && (
            <Button
              buttonStyle="full"
              text="LOGIN"
              type="button"
              onClick={() => auth.signinRedirect()}
            />
          )}
        </div>
      </div>
      <div className="flex-1 flex justify-center items-center p-16">
        <img className="w-3/4 h-auto" src={landingPage} alt="landing page" />
      </div>
    </div>
  );
};

export default HomeRoute;
