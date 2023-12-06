using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace User.Functions.Services
{
    public interface ICurrentUserService
    {
        void InitializeUser(IEnumerable<Claim> claims);
        Guid GetUserId();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private IEnumerable<Claim> Claims { get; set; }

        public CurrentUserService(IEnumerable<Claim> initialClaims)
        {
            Claims = initialClaims;
        }

        public Guid GetUserId()
        {
            if(Claims is null || !Claims.Any())
            {
                throw new ArgumentException("User claims not defined");
            }

            var guidAsString = Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (guidAsString != null) return Guid.Parse(guidAsString);
            throw new ArgumentException("User claims not defined");
        }

        public void InitializeUser(IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));
            Claims = claims;
            ValidateClaims();
        }

        private void ValidateClaims()
        {
            if(Claims.All(x => x.Type != ClaimTypes.NameIdentifier))
            {
                throw new ArgumentException("Provided claims dont contain user identifier");
            }
        }
    }
}
