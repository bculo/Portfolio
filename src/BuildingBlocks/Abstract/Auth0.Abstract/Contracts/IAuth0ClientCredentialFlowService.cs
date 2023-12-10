using Auth0.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Contracts
{
    public interface IAuth0ClientCredentialFlowService
    {
        
        Task<TokenClientCredentialResponse> GetToken(string clientId, string clientSecret, IEnumerable<string> scopes = default);
    }
}
