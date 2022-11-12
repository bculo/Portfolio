using AutoMapper;
using Crypto.Application.Interfaces.Services;
using Crypto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Resolvers
{
    public class IdentifierEncodeResolver : IValueResolver<Entity, object, string>
    {
        private readonly IIdentiferHasher _hasher;

        public IdentifierEncodeResolver(IIdentiferHasher hasher)
        {
            _hasher = hasher;
        }

        public string Resolve(Entity source, object destination, string destMember, ResolutionContext context)
        {
            return _hasher.Encode(source.Id);
        }
    }
}
