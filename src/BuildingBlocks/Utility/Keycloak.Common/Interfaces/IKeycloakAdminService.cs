using Keycloak.Common.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Interfaces
{
    /// <summary>
    /// Official keycloak admin api documentation https://www.keycloak.org/docs-api/18.0/rest-api/index.html#_clients_resource
    /// </summary>
    public interface IKeycloakAdminService
    {
        Task<List<UserResponse>> GetUsers(string realm, string accessToken);
        Task<UserResponse> GetUserById(string realm, string accessToken, Guid userId);
    }
}
