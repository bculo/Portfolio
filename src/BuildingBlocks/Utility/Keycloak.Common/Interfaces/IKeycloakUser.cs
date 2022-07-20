using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Interfaces
{
    public interface IKeycloakUser
    {
        bool IsAuthenticated();

        Guid? GetIdentifier();

        string? FullName();

        string? Email();

        string? UserName();

        string? GetIssuer();
    }
}
