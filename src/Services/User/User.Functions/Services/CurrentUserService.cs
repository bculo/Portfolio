using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            ClaimGuard();

            var guidAsString = _claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (guidAsString != null) return Guid.Parse(guidAsString);
            throw new ArgumentNullException(nameof(guidAsString));
        }

        public string GetUserName()
        {
            ClaimGuard();
            
            var userName = _claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return userName;
        }

        public void InitializeUser(IEnumerable<Claim> claims)
        {
            _claims = claims;
        }

        private void ClaimGuard()
        {
            if (_claims is null || !_claims.Any())
            {
                throw new InvalidOperationException("User claims already configured");
            }
        }
    }
}
