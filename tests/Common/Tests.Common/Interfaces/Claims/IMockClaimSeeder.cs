using System.Security.Claims;

namespace Tests.Common.Interfaces.Claims;

public interface IMockClaimSeeder
{
    IEnumerable<Claim> GetClaims(int userTypeId);
}