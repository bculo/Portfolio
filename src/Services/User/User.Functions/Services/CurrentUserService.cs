using System.Security.Claims;
using User.Application.Interfaces;

namespace User.Functions.Services
{

    public class CurrentUserService(IEnumerable<Claim> initialClaims) : ICurrentUserService
    {
        public Guid GetUserId()
        {
            ClaimGuard();

            var guidAsString = initialClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (guidAsString != null) return Guid.Parse(guidAsString);
            throw new ArgumentNullException(nameof(guidAsString));
        }

        public string GetUserName()
        {
            ClaimGuard();
            
            var userName = initialClaims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return userName;
        }

        public void InitializeUser(IEnumerable<Claim> claims)
        {
            initialClaims = claims;
        }

        private void ClaimGuard()
        {
            if (initialClaims is null || !initialClaims.Any())
            {
                throw new InvalidOperationException("User claims already configured");
            }
        }
    }
}
