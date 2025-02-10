using Auth0.Abstract.Models;

namespace Auth0.Abstract.Contracts
{
    public interface IAuth0ClientCredentialFlowService
    {
        Task<TokenClientCredentialResponse> GetToken(string clientId, 
            string clientSecret, 
            IEnumerable<string>? scopes = null);
    }
}
