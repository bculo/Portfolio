using System.Security.Claims;
using Tests.Common.Interfaces.Claims;
using Tests.Common.Interfaces.Claims.Models;

namespace Tests.Common.Services.Claims;

public class MockClaimSeeder : IMockClaimSeeder
{
    private readonly Dictionary<UserRole, List<Claim>> _claimDict = new()
    {
        {
            UserRole.None, []
        },
        {
            UserRole.User, [
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            ]
        },
        { 
            UserRole.Admin, [
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            ]
        }
    };

    public IEnumerable<Claim> GetClaims(int userTypeId)
    {
        if (!Enum.GetValues<UserRole>().Cast<int>().Contains(userTypeId))
        {
            throw new Exception("Given userTypeId is not enum");
        }
            
        return _claimDict[(UserRole)userTypeId];
    }
}