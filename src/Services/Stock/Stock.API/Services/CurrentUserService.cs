using Auth0.Abstract.Contracts;
using Stock.Application.Interfaces.User;

namespace Stock.API.Services
{
    public class CurrentUserService : IStockUser
    {
        private readonly IAuth0AccessTokenReader _user;

        public CurrentUserService(IAuth0AccessTokenReader user) => _user = user;

        public Guid Identifier
        {
            get
            {
                //var userId = _user.GetIdentifier();
                var userId = Guid.NewGuid();

                if(userId != Guid.Empty)
                {
                    return userId;
                }

                var msg =
                    "Problem with authentication. User identifier is null. Check if [Authorize] attribute is provided";
                throw new ArgumentException(msg);
            }
        }
    }
}
