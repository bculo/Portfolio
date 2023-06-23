using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace User.Functions.Services
{
    public interface IUserService
    {
        void InitializeUser(IEnumerable<Claim> claims);
        Guid GetUserId();
    }

    public class UserService : IUserService
    {
        public IEnumerable<Claim> Claims { get; private set; }

        public UserService(IEnumerable<Claim> initialClaims)
        {
            Claims = initialClaims;
        }

        public Guid GetUserId()
        {
            if(Claims is null || !Claims.Any())
            {
                throw new ArgumentException("User claims not defiend");
            }

            var guidAsString = Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(guidAsString);
        }

        public void InitializeUser(IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));
            Claims = claims;
            ValidateClaims();
        }

        private void ValidateClaims()
        {
            if(!Claims.Any(x => x.Type == ClaimTypes.NameIdentifier))
            {
                throw new ArgumentException("Provided claims dont contain user identifier");
            }
        }
    }
}
