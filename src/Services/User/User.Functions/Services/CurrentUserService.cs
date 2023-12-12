using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces;

namespace User.Functions.Services
{

    public class CurrentUserService : ICurrentUserService
    {
        private IEnumerable<Claim> _claims;

        public CurrentUserService(IEnumerable<Claim> initialClaims)
        {
            _claims = initialClaims;
        }

        public Guid GetUserId()
        {
            if(!_claims.Any())
            {
                throw new ArgumentException("User claims not defined");
            }

            var guidAsString = _claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (guidAsString != null) return Guid.Parse(guidAsString);
            throw new ArgumentException("User claims not defined");
        }

        public void InitializeUser(IEnumerable<Claim> claims)
        {
            if (_claims.Any())
            {
                throw new InvalidOperationException("User claims already configured");
            }
            
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));
            _claims = claims;
            ValidateClaims();
        }

        private void ValidateClaims()
        {
            if(_claims.All(x => x.Type != ClaimTypes.NameIdentifier))
            {
                throw new ArgumentException("Provided claims dont contain user identifier");
            }
        }
    }
}
