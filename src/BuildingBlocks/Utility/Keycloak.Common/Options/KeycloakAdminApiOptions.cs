using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Options
{
    public class KeycloakAdminApiOptions
    {
        public string Realm { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string AuthorizationUrl { get; set; } = default!;
        public string TokenBaseUri { get; set; } = default!;
        public string AdminApiBaseUri { get; set; } = default!;
    }
}
