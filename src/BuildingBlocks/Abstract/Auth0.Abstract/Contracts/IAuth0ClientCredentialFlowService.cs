using Auth0.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Contracts
{
    /// <summary>
    /// For more info check https://datatracker.ietf.org/doc/html/rfc6749
    /// </summary>
    public interface IAuth0ClientCredentialFlowService
    {
        /// <summary>
        /// Use this endpoint to directly request an Access Token by using the Client's credentials (a Client ID and a Client Secret).
        /// Authorization server, client credentials should be injected directly in implementation
        /// </summary>
        /// <param name="scopes">User scopes</param>
        /// <returns></returns>
        Task<TokenClientCredentialResponse> GetToken(string clientId, string clientSecret, IEnumerable<string>? scopes = default);
    }
}
