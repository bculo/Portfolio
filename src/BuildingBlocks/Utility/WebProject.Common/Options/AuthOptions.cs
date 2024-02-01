using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Common.Options
{
    public sealed class AuthOptions
    {
        public string PublicRsaKey { get; set; } = default!;
        public string ValidIssuer { get; set; } = default!;
        public bool ValidateAudience { get; set; } = default!;
        public bool ValidateIssuer { get; set; } = default!;
        public bool ValidateIssuerSigningKey { get; set; } = default!;
        public bool ValidateLifetime { get; set; } = default!;
    }
}
