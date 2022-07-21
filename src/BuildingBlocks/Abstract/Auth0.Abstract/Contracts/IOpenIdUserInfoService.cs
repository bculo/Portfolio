using Auth0.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Contracts
{
    /// <summary>
    /// More info https://openid.net/specs/openid-connect-core-1_0.html#UserInfo
    /// </summary>
    public interface IOpenIdUserInfoService
    {
        Task<UserInfoResponse> GetUserInfo(string accessToken);
    }
}
