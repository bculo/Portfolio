﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Entities
{
    public class PortfolioUser
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BornOn { get; set; }

        /// <summary>
        /// Keycloak ID in this case
        /// </summary>
        public Guid? ExternalId { get; set; }
    }
}
