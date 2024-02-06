using System.Security.Claims;
using Tests.Common.Interfaces.Claims;
using Tests.Common.Interfaces.Claims.Models;

namespace Tests.Common.Services.Claims;

public class MockClaimSeeder : IMockClaimSeeder
{
    private readonly Dictionary<UserRole, List<Claim>> _claimDict;

    public MockClaimSeeder()
    {
        _claimDict = new Dictionary<UserRole, List<Claim>>
        {
            {
                UserRole.None, new List<Claim>()
            },
            {
                UserRole.User, new List<Claim>()
                {
                    new (ClaimTypes.Role, "User"),
                    new (ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                } 
            },
            { 
                UserRole.Admin, new List<Claim>()
                {
                    new (ClaimTypes.Role, "Admin"),
                    new (ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                } 
            }
        };
    }
        
    public IEnumerable<Claim> GetClaims(int userTypeId)
    {
        if (!Enum.GetValues<UserRole>().Cast<int>().Contains(userTypeId))
        {
            throw new Exception("Given userTypeId is not enum");
        }
            
        return _claimDict[(UserRole)userTypeId];
    }
}