using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Contracts
{
    /// <summary>
    /// Interface for fetching data from Access token
    /// </summary>
    public interface IAuth0AccessTokenReader
    {
        bool IsAuthenticated();

        Guid GetIdentifier();

        string GetFullName();

        string GetEmail();

        string GetUserName();

        string GetIssuer();

        IEnumerable<string> GetRoles();

        bool IsInRole(string roleName);

        bool IsApplication();

        string GetClientId();

        IPAddress GetClientAddress();
    }
}
