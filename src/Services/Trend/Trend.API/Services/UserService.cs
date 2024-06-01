using Auth0.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Domain.Exceptions;

namespace Trend.API.Services;

public class UserService(IAuth0AccessTokenReader user) : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var userId = user.GetIdentifier();

            if(userId != Guid.Empty)
            {
                return userId;
            }

            const string message =
                "Problem with authentication. User identifier is null. Check if [Authorize] attribute is provided";
            throw new TrendAppCoreException(message);
        }
    }
}

