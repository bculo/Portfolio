using Auth0.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Domain.Exceptions;

namespace Trend.API.Services
{
    public class UserService : ICurrentUser
    {
        private readonly IAuth0AccessTokenReader _user;

        public UserService(IAuth0AccessTokenReader user) => _user = user;

        public Guid UserId
        {
            get
            {
                var userId = _user.GetIdentifier();

                if(userId != Guid.Empty)
                {
                    return userId;
                }

                var msg =
                    "Problem with authentication. User identifier is null. Check if [Authorize] attribute is provided";
                throw new TrendAppCoreException(msg);
            }
        }
    }
}
