using Auth0.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Domain.Exceptions;

namespace Trend.gRPC.Services
{
    public class UserService(IAuth0AccessTokenReader user) : ICurrentUser
    {
        public Guid UserId
        {
            get
            {
                var userId = user.GetIdentifier();

                if (userId == Guid.Empty)
                {
                    throw new TrendAppAuthenticationException("Problem with authentication. User identifier is null");
                }

                return userId;
            }
        }
    }
}
