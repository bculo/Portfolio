﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Models.Response
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string? Error { get; set; }
        [JsonProperty("error_description")]
        public string? ErrorDescription { get; set; }
    }
}
