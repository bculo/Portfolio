using Auth0.Abstract.Models;

namespace Auth0.Abstract.Contracts
{
    public interface IAuth0ResourceOwnerPasswordFlowService
    {
        Task<TokenAuthorizationCodeResponse> GetToken(string clientId, 
            string username, 
            string password, 
            IEnumerable<string>? scopes = null);
    }
}
