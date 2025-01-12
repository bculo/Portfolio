using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Services.Models
{
    public class CredentialRepresentation
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Temporary { get; set; }
    }
}
