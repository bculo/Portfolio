using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Options
{
    public class KeycloakAdminApiOptions
    {
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenBaseUri { get; set; }
        public string AdminApiBaseUri { get; set; }
    }
}
