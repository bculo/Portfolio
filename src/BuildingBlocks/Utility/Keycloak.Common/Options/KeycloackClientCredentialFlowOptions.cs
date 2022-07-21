using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Options
{
    internal class KeycloackClientCredentialFlowOptions
    {
        public string? AuthorizationServerUrl { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
