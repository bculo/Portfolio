using System.Security.Claims;

namespace User.Application.Interfaces;

public interface ICurrentUserService
{
    void InitializeUser(IEnumerable<Claim> claims);
    Guid GetUserId();

    string GetUserName();
}