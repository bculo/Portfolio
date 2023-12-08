using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Common.Options
{
    public sealed class AuthOptions
    {
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenBaseUri { get; set; }
        public string AdminApiBaseUri { get; set; }
    }
}
