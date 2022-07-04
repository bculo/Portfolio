using AutoMapper;
using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Common.Mappings
{
    public class AttachTimeAction : IMappingAction<object, IAuditableEntity>
    {
        private readonly IDateTime _time;

        public AttachTimeAction(IDateTime time)
        {
            _time = time;
        }

        public void Process(object source, IAuditableEntity destination, ResolutionContext context)
        {
            var time = _time.DateTime;

            destination.CreatedOn = time;
            destination.ModifiedOn = time;
        }
    }
}
