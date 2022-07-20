using Keycloak.Common.Interfaces;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.API.Services
{
    public class UserService : ICurrentUser
    {
        private readonly IKeycloakUser _user;

        public UserService(IKeycloakUser user)
        {
            _user = user;
        }

        public Guid UserId
        {
            get
            {
                var userId = _user.GetIdentifier();

                if(userId is null)
                {
                    throw new TrendAppAuthenticationException("Problem with authentication. User identifier is null");
                }

                return UserId;
            }
        }
    }
}
