using Auth0.Abstract.Contracts;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.Grpc.Services
{
    public class UserService : ICurrentUser
    {
        private readonly IAuth0AccessTokenReader _user;

        public UserService(IAuth0AccessTokenReader user)
        {
            _user = user;
        }

        public Guid UserId
        {
            get
            {
                var userId = _user.GetIdentifier();

                if (userId == Guid.Empty)
                {
                    throw new TrendAppAuthenticationException("Problem with authentication. User identifier is null");
                }

                return UserId;
            }
        }
    }
}
