using Auth0.Abstract.Contracts;
using Stock.Application.Interfaces.User;
using Stock.Core.Exceptions;

namespace Stock.API.Services
{
    public class CurrentUserService(IAuth0AccessTokenReader user) : IStockUser
    {
        public Guid Identifier
        {
            get
            {
                var userId = user.GetIdentifier();

                if(userId != Guid.Empty)
                {
                    return userId;
                }

                const string msg = 
                    "Problem with authentication. User identifier is null. Check if [Authorize] attribute is provided";
                throw new StockCoreAuthException(msg);
            }
        }
    }
}
