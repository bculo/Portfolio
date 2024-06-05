using Auth0.Abstract.Contracts;
using Stock.Application.Interfaces.User;
using Stock.Core.Exceptions;

namespace Stock.gRPC.Services;

public class UserService(IAuth0AccessTokenReader user) : IStockUser
{
    private readonly IAuth0AccessTokenReader _user = user;

    public Guid Identifier
    {
        get
        {
            var userId = _user.GetIdentifier();

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