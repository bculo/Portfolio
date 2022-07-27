using Auth0.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Contracts
{
    public interface IAuth0OwnerCredentialFlowService
    {
        /// <summary>
        /// Get access token using Resource Owner Password Credentials Grant
        /// </summary>
        /// <param name="clientId">Resource ID</param>
        /// <param name="username">Registered username</param>
        /// <param name="password">User password</param>
        /// <param name="scopes">Scopes</param>
        /// <returns></returns>
        Task<TokenClientCredentialResponse> GetToken(string clientId, string username, string password, IEnumerable<string>? scopes = default);
    }
}
