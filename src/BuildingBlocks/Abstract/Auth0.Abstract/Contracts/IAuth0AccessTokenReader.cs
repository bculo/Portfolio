using System.Net;

namespace Auth0.Abstract.Contracts
{
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