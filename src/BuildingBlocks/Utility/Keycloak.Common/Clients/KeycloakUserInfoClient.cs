using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Clients
{
    internal class KeycloakUserInfoClient : IOpenIdUserInfoService
    {
        public Task<UserInfoResponse> GetUserInfo(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
