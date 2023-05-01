using System.Security.Claims;

namespace Tests.Common.Interfaces;

public interface IMockClaimSeeder
{
    IEnumerable<Claim> GetClaims(int userTypeId);
}